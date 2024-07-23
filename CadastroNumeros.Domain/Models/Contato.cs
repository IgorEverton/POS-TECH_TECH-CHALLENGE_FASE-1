using CadastroNumeros.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace CadastroNumeros.Domain.Models;

public class Contato
{
    public Guid Id { get; set; }

    public DateTime DataCriacao { get; set; }
    
    [MaxLength(100)]
    public string Nome { get; set; }

    public int? Idade { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(9)]
    public string Telefone { get; set; }

    [DddValidation]
    public int CodigoDdd { get; set; }
}

