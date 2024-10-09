using CadastroNumeros.Domain.Models;

namespace CadastroNumeros.Infra.Interfaces.Service
{
    public interface IContatoService
    {
        public Task<IEnumerable<Contato>> ListarContatosPorDddAsync(int ddd);
        public Task<IEnumerable<Contato>> ListarContatosAsync(int pageNumber, int pageSize);
        public Task<Contato> RetornarContatoAsync(Guid id);
        public Task<Contato> CriarContatoAsync(Contato contato);
        public Task<int> AtualizarContatoAsync(Contato contato);
        public Task DeletarContatoAsync(Guid Id);
    }
}
