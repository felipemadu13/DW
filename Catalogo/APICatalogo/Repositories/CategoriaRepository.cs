using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {        
    }

    // public IEnumerable<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
    // {
    //     return GetAll()
    //         .OrderBy(on => on.Nome)
    //         .Skip((categoriasParameters.PageNumber - 1) * categoriasParameters.PageSize)
    //         .Take(categoriasParameters.PageSize).ToList();
    // }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
    {
        var categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();
        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

        return categoriasOrdenadas;
    }

    public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroNome)
    {
        var categorias = GetAll().AsQueryable();
         categorias = categorias.Where(p => p.Nome.ToUpper().Contains(categoriasFiltroNome.nomeBusca.ToUpper()));

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);

        return categoriasFiltradas;
    }
}
