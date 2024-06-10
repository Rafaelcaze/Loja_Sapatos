using System.ComponentModel.DataAnnotations;

namespace PeDeOuro.Models;

public class Produto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    public decimal Preco { get; set; }
}