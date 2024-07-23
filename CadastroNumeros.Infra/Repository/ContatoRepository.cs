using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CadastroNumeros.Implementations;

public class ContatoRepository : IContatoRepository
{
    private readonly AppDbContext _context;
    public ContatoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Método responsavel por cadastrar um usuário no banco de dados
    /// </summary>
    /// <param name="contato">Objeto do tipo Contato</param>
    /// <returns>Objeto do tipo Contato com as informações
    /// cadastradas no banco de dados
    /// </returns>
    public async Task<Contato> CriarContato(Contato contato)
    {
        contato.DataCriacao = DateTime.Now.ToLocalTime();

        await _context.AddAsync(contato);
        _context.SaveChanges();
        return contato;
    }

    /// <summary>
    /// Método responsável por retornar um usuário de acordo com o ID
    /// </summary>
    /// <param name="id">Id do Contato</param>
    /// <returns>Retornar um objeto do tipo usuário 
    /// com as informações cadastradas no banco
    /// /returns>
    public async Task<Contato> RetornarContato(Guid id)
    {
        return await _context.Contatos.FindAsync(id);
    }

    /// <summary>
    /// Método resposável por retornar todos os usuários cadastrados no banco de dados
    /// </summary>
    /// <returns>Lista de objetos do tipo Contato</returns>
    public async Task<IEnumerable<Contato>> ListarContatos()
    {
        return await _context.Contatos.ToListAsync();
    }

    /// <summary>
    /// Método resposável por retornar contatos cadastrados no banco de dados por ddd
    /// </summary>
    /// <returns>Lista de objetos do tipo Contato</returns>
    public async Task<IEnumerable<Contato>> ListarContatosPorDdd(int ddd)
    {
        return await _context.Contatos.Where(x => x.CodigoDdd == ddd).ToListAsync();
    }

    /// <summary>
    /// Método responsável por atualizar as informações de um contato no banco de dados
    /// </summary>
    /// <param name="contato">Um objeto do tipo Contato</param>
    public async Task AtualizarContato(Contato contato)
    {
        _context.Contatos.Update(contato);
        _context.SaveChanges();
    }

    /// <summary>
    /// Método resposável por excluir um contato do banco de dados
    /// </summary>
    /// <param name="Id">Id do Contato</param>
    public async Task DeletarContato(Guid id)
    {
        var contatoEncontrado = await _context.Contatos.FindAsync(id);
        if (contatoEncontrado != null)
        {
            _context.Remove(contatoEncontrado);
            _context.SaveChanges();
        }
        else
        {
            throw new Exception("Contato não encontrado na base de dados");
        }
    }
}