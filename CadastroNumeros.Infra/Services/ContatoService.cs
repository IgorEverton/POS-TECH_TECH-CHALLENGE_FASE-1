using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Queues;
using CadastroNumeros.Domain.Configuration.Queues.RabbitMQ;
using CadastroNumeros.Domain.Configuration.CustomMessages;

namespace CadastroNumeros.Infra.Services;

public class ContatoService : ServiceBase, IContatoService
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IRabbitMQPublisher<CadastroSolicitacao> _contatoPublisher;

    public ContatoService(IContatoRepository contatoRepository, IRabbitMQPublisher<CadastroSolicitacao> publisher)
    {
        _contatoRepository = contatoRepository;
        _contatoPublisher = publisher;
    }
    public async Task<SolicitacaoResult> AtualizarContato(Contato contato)
    {
        try
        {
            var solicitacao = new CadastroSolicitacao() { Contato = contato, TipoSolicitacao = Domain.Enum.TipoSolicitacao.Alterar };

            await _contatoPublisher.PublishMessageAsync(solicitacao, RabbitMQQueues.CadastroContatoQueue);

            return RetornaResultado(ReturnMessages.SolicitacaoRealizada);
        }
        catch (Exception ex)
        {
            return RetornaResultado($"{ReturnMessages.SolicitacaoNaoRealizada}{ex.Message}", false);
        }
    }

    public async Task<SolicitacaoResult> CriarContato(Contato contato)
    {
        try
        {
            var solicitacao = new CadastroSolicitacao() { Contato = contato, TipoSolicitacao = Domain.Enum.TipoSolicitacao.Inserir };

            await _contatoPublisher.PublishMessageAsync(solicitacao, RabbitMQQueues.CadastroContatoQueue);

            return RetornaResultado(ReturnMessages.SolicitacaoRealizada);
        }
        catch (Exception ex)
        {
            return RetornaResultado($"{ReturnMessages.SolicitacaoNaoRealizada}{ex.Message}", false);
        }
    }

    public async Task<SolicitacaoResult> DeletarContato(Guid id)
    {
        try
        {
            var solicitacao = new CadastroSolicitacao() { Contato = new() { Id = id}, TipoSolicitacao = Domain.Enum.TipoSolicitacao.Alterar };

            await _contatoPublisher.PublishMessageAsync(solicitacao, RabbitMQQueues.CadastroContatoQueue);

            return RetornaResultado(ReturnMessages.SolicitacaoRealizada);
        }
        catch (Exception ex)
        {
            return RetornaResultado($"{ReturnMessages.SolicitacaoNaoRealizada}{ex.Message}", false);
        }
    }

    public async Task<IEnumerable<Contato>> ListarContatos(int pageNumber = 1, int pageSize = 10)
    {
        return await _contatoRepository.ListarContatos(pageNumber, pageSize);
    }

    public async Task<IEnumerable<Contato>> ListarContatosPorDdd(int ddd)
    {
        return await _contatoRepository.ListarContatosPorDdd(ddd);
    }

    public async Task<Contato> RetornarContato(Guid id)
    {
        return await _contatoRepository.RetornarContato(id);
    }
}
