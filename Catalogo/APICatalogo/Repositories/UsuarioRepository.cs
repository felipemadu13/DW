using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context)
    {        
    }

    // public IEnumerable<Usuario> GetUsuarios(UsuariosParameters usuariosParameters)
    // {
    //     return GetAll()
    //         .OrderBy(on => on.Nome)
    //         .Skip((usuariosParameters.PageNumber - 1) * usuariosParameters.PageSize)
    //         .Take(usuariosParameters.PageSize).ToList(); 
    // }
    public PagedList<Usuario> GetUsuarios(UsuariosParameters usuariosParameters)
    {
        var usuarios = GetAll().OrderBy(p => p.UsuarioId).AsQueryable();
        var usuariosOrdenados = PagedList<Usuario>.ToPagedList(usuarios, usuariosParameters.PageNumber, usuariosParameters.PageSize);

        return usuariosOrdenados;
    }

        public PagedList<Usuario> GetUsuariosFiltroNome(UsuariosFiltroNome usuariosFiltroNome)
    {
        var usuarios = GetAll().AsQueryable();
        usuarios = usuarios.Where(p => p.Nome.ToUpper().Contains(usuariosFiltroNome.nomeBusca.ToUpper()));

        var usuariosFiltrados = PagedList<Usuario>.ToPagedList(usuarios, usuariosFiltroNome.PageNumber, usuariosFiltroNome.PageSize);

        return usuariosFiltrados;
    }

}
