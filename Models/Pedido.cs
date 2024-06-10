using System.ComponentModel.DataAnnotations;

namespace PeDeOuro.Models;

public class Pedido
{
    public int Id { get; set; }

    [Required(ErrorMessage = "É necessário informar pelos menos um produto.")]
    public decimal Total { get; set; }

    public int? ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    [Required(ErrorMessage = "É necessário selecionar pelo menos um produto.")]
    public List<ItensPedido> ItensPedido { get; set; } = new List<ItensPedido>();

}