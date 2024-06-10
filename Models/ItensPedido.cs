using System.ComponentModel.DataAnnotations;

namespace PeDeOutro.Models;

public class ItensPedido 
{
    public int Id { get; set; }
    public int PedidoId { get; set; }

    public Pedido Pedido { get; set; }
    public int ProdutoId { get; set; }

    public Produto Produto { get; set; }
}