namespace CadastroNumeros.Domain.Models
{
    public class SolicitacaoResult
    {
        public required bool Sucesso { get; set; }
        public required string Mensagem { get; set; } = string.Empty;
    }
}
