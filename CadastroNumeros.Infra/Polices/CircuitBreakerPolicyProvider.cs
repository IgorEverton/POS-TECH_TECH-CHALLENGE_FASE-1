using CadastroNumeros.Infra.Interfaces.Polices;
using Polly;
using Polly.CircuitBreaker;

namespace CadastroNumeros.Infra.Polices
{
    public class CircuitBreakerPolicyProvider : ICircuitBreakerPolicyProvider
    {
        private readonly AsyncCircuitBreakerPolicy _policy;

        public CircuitBreakerPolicyProvider()
        {
            _policy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3, // Número de falhas consecutivas antes de abrir o circuito
                    durationOfBreak: TimeSpan.FromSeconds(30), // Tempo que o circuito ficará aberto
                    onBreak: (exception, duration) =>
                    {
                        Console.WriteLine($"Circuito aberto devido a: {exception.Message}. Aguardando {duration.TotalSeconds} segundos.");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuito fechado. Operações retomadas.");
                    },
                    onHalfOpen: () =>
                    {
                        Console.WriteLine("Circuito em estado de teste (meio-aberto).");
                    }
                );
        }

        public AsyncCircuitBreakerPolicy GetPolicy() => _policy;
    }
}
