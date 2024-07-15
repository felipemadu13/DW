using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
    PagedList<Produto> GetProdutos(QueryStringParameters queryStringParameters);
    PagedList<Produto> GetProdutosFiltroPreco(PrecoFilter precoFilter);
}