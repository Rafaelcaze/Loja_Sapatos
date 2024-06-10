using System.ComponentModel.DataAnnotations;

namespace PeDeOutro.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "O campo Endereço é obrigatório.")]
    public string? Endereco { get; set; }
}