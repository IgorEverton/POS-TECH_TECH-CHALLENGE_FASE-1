using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Queues;
using CadastroNumeros.Domain.Configuration.Queues.RabbitMQ;
using CadastroNumeros.Domain.Configuration.CustomMessages;
using Polly.CircuitBreaker;
using CadastroNumeros.Infra.Polices;
using CadastroNumeros.Infra.Interfaces.Polices;

namespace CadastroNumeros.Infra.Services;

public class ContatoService : ServiceBase, IContatoService
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IRabbitMQPublisher<CadastroSolicitacao> _contatoPublisher;
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

    public ContatoService(IContatoRepository contatoRepository, 
                          IRabbitMQPublisher<CadastroSolicitacao> publisher,
                          ICircuitBreakerPolicyProvider policyProvider)
    {
        _contatoRepository = contatoRepository;
        _contatoPublisher = publisher;
        _circuitBreakerPolicy = policyProvider.GetPolicy();
    }
    public async Task<SolicitacaoResult> AtualizarContato(Contato contato)
    {
        try
        {
            var solicitacao = new CadastroSolicitacao() { Contato = contato, TipoSolicitacao = Domain.Enum.TipoSolicitacao.Alterar };

            await _circuitBreakerPolicy.ExecuteAsync(() =>
                    _contatoPublisher.PublishMessageAsync(solicitacao, RabbitMQQueues.CadastroContatoQueue));

            return RetornaResultado(ReturnMessages.SolicitacaoRealizada);
        }
        catch (BrokenCircuitException)
        {
            return RetornaResultado(ReturnMessages.ServicoIndisponivel, false);
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

            // Execute a chamada à fila RabbitMQ com o Circuit Breaker
            await _circuitBreakerPolicy.ExecuteAsync(() =>
                    _contatoPublisher.PublishMessageAsync(solicitacao, RabbitMQQueues.CadastroContatoQueue));

            return RetornaResultado(ReturnMessages.SolicitacaoRealizada);
        }
        catch (BrokenCircuitException)
        {
            return RetornaResultado(ReturnMessages.ServicoIndisponivel, false);
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
            var solicitacao = new CadastroSolicitacao() { Contato = new() { Id = id}, TipoSolicitacao = Domain.Enum.TipoSolicitacao.Deletar };

            await _circuitBreakerPolicy.ExecuteAsync(() =>
                    _contatoPublisher.PublishMessageAsync(solicitacao, RabbitMQQueues.CadastroContatoQueue));

            return RetornaResultado(ReturnMessages.SolicitacaoRealizada);
        }
        catch (BrokenCircuitException)
        {
            return RetornaResultado(ReturnMessages.ServicoIndisponivel, false);
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
