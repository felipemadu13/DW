﻿using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }


    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _uof.ProdutoRepository.GetAll();
        if (produtos is null)
        {
            return NotFound();
        }
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _uof.ProdutoRepository.Get(c => c.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        var produtoDto = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtoDto);
    }

    [HttpGet("categoria/{id}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
            return NotFound();

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDto);

        var novoProduto = _uof.ProdutoRepository.Create(produto);
        _uof.Commit();

        var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
            return BadRequest();//400

        var produto = _mapper.Map<Produto>(produtoDto);

        var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }

        var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();

        var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produtoDeletado);

        return Ok(produtoDeletadoDto);
    }


    [HttpPatch("{id}/UpdatePartial")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id,
    JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO is null || id <= 0)
            return BadRequest();

        var produto = _uof.ProdutoRepository.Get(c => c.ProdutoId == id);

        if (produto is null)
            return NotFound();
        
        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
        patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);
        if(!(ModelState.IsValid || TryValidateModel(produtoUpdateRequest)))
            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest, produto);

        _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] QueryStringParameters queryStringParameters)
    {
        var produtos = _uof.ProdutoRepository.GetProdutos(queryStringParameters);
        return ObterProdutos(produtos);
    }

    [HttpGet("filter/preco/pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFilterPreco([FromQuery] PrecoFilter precoFilter)
    {
        var produtos = _uof.ProdutoRepository.GetProdutosFiltroPreco(precoFilter);
        return ObterProdutos(produtos); 
    }

    [HttpGet("nome/categoria/")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdPorCategoria([FromQuery] NomeFilter nomeFilter)
    {
        var categorias = _uof.CategoriaRepository.GetCategoriasFiltroNome(nomeFilter);
        var idCategoria = categorias.FirstOrDefault().CategoriaId;

        var produtos = _uof.ProdutoRepository.GetProdutosPorCategoriaNome(idCategoria, nomeFilter); 

        if (produtos is null)
            return NotFound();

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);   
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
    {
         var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);

    }


}