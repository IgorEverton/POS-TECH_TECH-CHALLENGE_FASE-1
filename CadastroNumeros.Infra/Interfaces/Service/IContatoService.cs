using CadastroNumeros.Domain.Models;

namespace CadastroNumeros.Infra.Interfaces.Service
{
    public interface IContatoService
    {
        public Task<IEnumerable<Contato>> ListarContatosPorDdd(int ddd);
        public Task<IEnumerable<Contato>> ListarContatos(int pageNumber, int pageSize);
        public Task<Contato> RetornarContato(Guid id);
        public Task<Contato> CriarContato(Contato contato);
        public Task<int> AtualizarContato(Contato contato);
        public Task DeletarContato(Guid Id);
    }
}
