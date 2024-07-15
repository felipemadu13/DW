using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context): base(context)
    {       
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }

    public PagedList<Produto> GetProdutos(QueryStringParameters queryStringParameters)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, queryStringParameters.PageNumber, queryStringParameters.PageSize);

        return produtosOrdenados;
    }

    public PagedList<Produto> GetProdutosFiltroPreco(PrecoFilter precoFilter)
    {
        var produtos = GetAll().AsQueryable();

        if (precoFilter.Preco.HasValue && !string.IsNullOrEmpty(precoFilter.PrecoCriterio))
        {
            if (precoFilter.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco > precoFilter.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (precoFilter.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco < precoFilter.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (precoFilter.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == precoFilter.Preco.Value).OrderBy(p => p.Preco);
            }
        }

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, precoFilter.PageNumber, precoFilter.PageSize);

        return produtosFiltrados;
    }
}