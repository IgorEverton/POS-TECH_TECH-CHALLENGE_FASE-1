using CadastroNumeros.Domain.Models;

namespace CadastroNumeros.Infra.Interfaces.Service
{
    public interface IContatoService
    {
        public Task<IEnumerable<Contato>> ListarContatosPorDdd(int ddd);
        public Task<IEnumerable<Contato>> ListarContatos(int pageNumber, int pageSize);
        public Task<Contato> RetornarContato(Guid id);
        public Task<SolicitacaoResult> CriarContato(Contato contato);
        public Task<SolicitacaoResult> AtualizarContato(Contato contato);
        public Task<SolicitacaoResult> DeletarContato(Guid Id);
    }
}
