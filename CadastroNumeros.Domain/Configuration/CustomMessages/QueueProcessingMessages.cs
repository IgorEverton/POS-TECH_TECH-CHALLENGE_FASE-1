namespace CadastroNumeros.Domain.Configuration.CustomMessages
{
    public class QueueProcessingMessages
    {
        public const string AcaoInvalida = "Tipo de ação invalido";
        public const string OperacaoRealizadaSucesso = "Operação realizada com sucesso!";
        public const string ErroProcessarQueue = "Ocorreu um erro ao processar a mensagem da fila";
        public const string ConsumoIniciado = "Iniciando o consumo das mensagens da fila!";
    }
}
