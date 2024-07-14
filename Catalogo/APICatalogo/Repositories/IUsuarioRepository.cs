using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
     // IEnumerable<Usuario> GetUsuarios(UsuariosParameters usuariosParameters);
    PagedList<Usuario> GetUsuarios(UsuariosParameters usuariosParameters);
    PagedList<Usuario> GetUsuariosFiltroNome(UsuariosFiltroNome usuariosFiltroNome);
}
