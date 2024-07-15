using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(QueryStringParameters queryStringParameters);
    PagedList<Categoria> GetCategoriasFiltroNome(NomeFilter nomeFilter);
}
