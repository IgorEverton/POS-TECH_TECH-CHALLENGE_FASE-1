using CadastroNumeros.Domain.Models;

namespace CadastroNumeros.Infra.Interfaces.Repository
{
    public interface IContatoRepository
    {
        public Task<IEnumerable<Contato>> ListarContatosPorDdd(int ddd);
        public Task<IEnumerable<Contato>> ListarContatos();
        public Task<Contato> RetornarContato(Guid id);
        public Task<Contato> CriarContato(Contato contato);
        public Task AtualizarContato(Contato contato);
        public Task DeletarContato(Guid Id);
    }
}
