using APICatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.DTOs;

public class UsuarioDTO
{
    public int UsuarioId { get; set; }

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Senha { get; set; }

}
