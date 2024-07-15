using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuariosController : ControllerBase {

    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public UsuariosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UsuarioDTO>> Get()
    {
        var usuarios = _uof.UsuarioRepository.GetAll();

        if (usuarios is null)
            return NotFound("Não existem usuarios...");

        var usuariosDTO = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
        return Ok(usuariosDTO);

    }

    [HttpGet("{id}", Name = "ObterUsuario")]
    public ActionResult<UsuarioDTO> Get(int id)
    {
        var usuario = _uof.UsuarioRepository.Get(c => c.UsuarioId == id);
        if (usuario is null)
        {
            return NotFound("Usuario não encontrado...");
        }
        var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
        return Ok(usuarioDTO);
    }

    [HttpPost]
    public ActionResult<UsuarioDTO> Post(UsuarioDTO usuarioDTO)
    {
         if (usuarioDTO is null)
            return BadRequest();

        var usuario = _mapper.Map<Usuario>(usuarioDTO);

        var novoUsuario = _uof.UsuarioRepository.Create(usuario);
        _uof.Commit();

        var novousuarioDTO = _mapper.Map<UsuarioDTO>(novoUsuario);

        return new CreatedAtRouteResult("ObterUsuario",
            new { id = novousuarioDTO.UsuarioId }, novousuarioDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult<UsuarioDTO> Put(int id, UsuarioDTO usuarioDTO)
    {
        if (id != usuarioDTO.UsuarioId)
            return BadRequest();//400

        var usuario = _mapper.Map<Usuario>(usuarioDTO);

        var usuarioAtualizado = _uof.UsuarioRepository.Update(usuario);
        _uof.Commit();

        var usuarioAtualizadoDTO = _mapper.Map<UsuarioDTO>(usuarioAtualizado);

        return Ok(usuarioAtualizadoDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<UsuarioDTO> Delete(int id)
    {
        var usuario = _uof.UsuarioRepository.Get(p => p.UsuarioId == id);
        if (usuario is null)
        {
            return NotFound("Usuario não encontrado...");
        }

        var usuarioDeletado = _uof.UsuarioRepository.Delete(usuario);
        _uof.Commit();

        var usuarioDeletadoDTO = _mapper.Map<UsuarioDTO>(usuarioDeletado);

        return Ok(usuarioDeletadoDTO);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<UsuarioDTO>> Get([FromQuery] QueryStringParameters queryStringParameters)
    {
        var usuarios = _uof.UsuarioRepository.GetUsuarios(queryStringParameters);

        var metadata = new
        {
            usuarios.TotalCount,
            usuarios.PageSize,
            usuarios.CurrentPage,
            usuarios.TotalPages,
            usuarios.HasNext,
            usuarios.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        var usuariosDTO = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
        return Ok(usuariosDTO);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<UsuarioDTO>> GetUsuariosFilterNome([FromQuery] NomeFilter nomeFilter)
    {
         var usuarios = _uof.UsuarioRepository.GetUsuariosFiltroNome(nomeFilter);

        var metadata = new
        {
            usuarios.TotalCount,
            usuarios.PageSize,
            usuarios.CurrentPage,
            usuarios.TotalPages,
            usuarios.HasNext,
            usuarios.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        var usuariosDTO = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
        return Ok(usuariosDTO);

    }

}