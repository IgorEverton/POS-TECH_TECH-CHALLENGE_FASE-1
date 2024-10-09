using CadastroNumeros.Domain.Models;

namespace CadastroNumeros.Infra.Interfaces.Repository
{
    public interface IContatoRepository
    {
        public Task<IEnumerable<Contato>> ListarContatosPorDddAsync(int ddd);
        public Task<IEnumerable<Contato>> ListarContatosAsync(int pageNumber, int pageSize);
        public Task<Contato> RetornarContatoAsync(Guid id);
        public Task<Contato> CriarContatoAsync(Contato contato);
        public Task<int> AtualizarContatoAsync(Contato contato);
        public Task DeletarContatoAsync(Guid Id);
    }
}
