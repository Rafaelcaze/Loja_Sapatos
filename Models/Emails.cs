using System.ComponentModel.DataAnnotations;

namespace PeDeOuro.Models;

public class Emails
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O campo PromocaoId é obrigatório")]
    public int PromocaoId { get; set; }
    public string emailDestinatario { get; set; }

    //public List<Cliente> clientes { get; set; }
}