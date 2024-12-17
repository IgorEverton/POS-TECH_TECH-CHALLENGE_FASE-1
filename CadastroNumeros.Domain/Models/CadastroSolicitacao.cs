using CadastroNumeros.Domain.Enum;

namespace CadastroNumeros.Domain.Models
{
    public class CadastroSolicitacao
    {
        public required TipoSolicitacao TipoSolicitacao { get; set; }
        public required Contato Contato { get; set; }
    }
}
