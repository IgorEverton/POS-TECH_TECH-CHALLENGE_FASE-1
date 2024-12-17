using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Service;

namespace CadastroNumeros.Infra.Services
{
    public abstract class ServiceBase : IServiceBase
    {
        public SolicitacaoResult RetornaResultado(string mensagem, bool sucesso = true)
        {
            return new SolicitacaoResult() { Mensagem = mensagem, Sucesso = sucesso};
        }
    }
}
