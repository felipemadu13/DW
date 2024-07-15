using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context)
    {        
    }
    public PagedList<Usuario> GetUsuarios(QueryStringParameters queryStringParameters)
    {
        var usuarios = GetAll().OrderBy(p => p.UsuarioId).AsQueryable();
        var usuariosOrdenados = PagedList<Usuario>.ToPagedList(usuarios, queryStringParameters.PageNumber, queryStringParameters.PageSize);

        return usuariosOrdenados;
    }

        public PagedList<Usuario> GetUsuariosFiltroNome(NomeFilter nomeFilter)
    {
        var usuarios = GetAll().AsQueryable();
        usuarios = usuarios.Where(p => p.Nome.ToUpper().Contains( nomeFilter.nome.ToUpper()));

        var usuariosFiltrados = PagedList<Usuario>.ToPagedList(usuarios,  nomeFilter.PageNumber,  nomeFilter.PageSize);

        return usuariosFiltrados;
    }

}
