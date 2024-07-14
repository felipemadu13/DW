using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Usuarios")]
public class Usuario
{
     [Key]
    public int UsuarioId { get; set; }

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Senha { get; set; }

}