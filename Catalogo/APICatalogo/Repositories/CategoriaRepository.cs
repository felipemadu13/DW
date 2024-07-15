using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {        
    }

    public PagedList<Categoria> GetCategorias(QueryStringParameters queryStringParameters)
    {
        var categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();
        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, queryStringParameters.PageNumber, queryStringParameters.PageSize);

        return categoriasOrdenadas;
    }

    public PagedList<Categoria> GetCategoriasFiltroNome(NomeFilter nomeFilter)
    {
        var categorias = GetAll().AsQueryable();
         categorias = categorias.Where(p => p.Nome.ToUpper().Contains(nomeFilter.nome.ToUpper()));

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, nomeFilter.PageNumber, nomeFilter.PageSize);

        return categoriasFiltradas;
    }
}
