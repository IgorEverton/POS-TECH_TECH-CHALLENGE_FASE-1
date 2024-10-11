using CadastroNumeros.Domain.Validation;
using CadastroNumeros.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace CadastroNumeros.Domain.Models;

public class Contato
{
    public virtual Guid Id { get; protected set; }

    public virtual DateTime DataCriacao { get; protected set; }
    
    [MaxLength(100)]
    public virtual string Nome { get; protected set; }

    public virtual int? Idade { get; protected set; }

    //[EmailAddress]
    [MaxLength(100)]
    public virtual Email Email { get; private set; }

    [MaxLength(9)]
    public virtual string Telefone { get; protected set; }

    [DddValidation]
    public virtual int CodigoDdd { get; protected set; }

    public Contato() { }

    public Contato(Guid id, DateTime dataCriacao, string nome, int? idade, string telefone, Email email, int codigoDdd)
    {
        SetId(id);
        SetDataCriacao(dataCriacao);
        SetNome(nome);
        SetIdade(idade);
        SetTelefone(telefone);
        SetEmail(email);
        SetCodigoDdd(codigoDdd);
    }

    public virtual void SetId(Guid id)
    {
        try
        { 
            if (string.IsNullOrEmpty(id.GetType().GUID.ToString()))
                throw new Exception("Nome inválido");

            Id = id;
        }
        catch
        {
            throw;
        }
    }

    public virtual void SetDataCriacao(DateTime dataCriacao)
    {
        try
        {
            if (string.IsNullOrEmpty(dataCriacao.ToString()))
                throw new Exception("Nome inválido");

            DataCriacao = dataCriacao;
        }
        catch
        {
            throw;
        }
    }

    public virtual void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrEmpty(nome))
            throw new Exception("Nome inválido");

        Nome = nome;
    }

    public virtual void SetIdade(int? idade)
    {
        if (idade <= 0)
            throw new Exception("Idade inválida");

        Idade = idade;
    }

    public virtual void SetTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrEmpty(telefone))
            throw new Exception("Telefone inválido");

        Telefone = telefone;
    }

    public virtual void SetEmail(Email email)
    {
        Email = email ?? throw new Exception("Email inválido");
    }

    public virtual void SetCodigoDdd(int codigoDdd)
    {
        CodigoDdd = codigoDdd <= 0 ? throw new Exception("Email inválido") : codigoDdd;
    }

}

