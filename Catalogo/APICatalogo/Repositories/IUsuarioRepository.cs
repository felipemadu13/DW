using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
     // IEnumerable<Usuario> GetUsuarios(UsuariosParameters usuariosParameters);
    PagedList<Usuario> GetUsuarios(QueryStringParameters queryStringParameters);
    PagedList<Usuario> GetUsuariosFiltroNome(NomeFilter nomeFilter);
}
