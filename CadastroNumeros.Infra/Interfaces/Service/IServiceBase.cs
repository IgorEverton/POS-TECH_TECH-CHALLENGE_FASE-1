using CadastroNumeros.Domain.Models;

namespace CadastroNumeros.Infra.Interfaces.Service
{
    public interface IServiceBase
    {
        SolicitacaoResult RetornaResultado(string mensagem, bool sucesso = true);
    }
}
