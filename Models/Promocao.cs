using System.ComponentModel.DataAnnotations;

namespace PeDeOuro.Models;

public class Promocao
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Assunto é obrigatório")]
    public string Assunto { get; set; }

    [Required(ErrorMessage = "O campo Descrição é obrigatório")]
    public string Descricao { get; set; }
}