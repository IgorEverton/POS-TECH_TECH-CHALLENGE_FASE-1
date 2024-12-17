using Polly.CircuitBreaker;

namespace CadastroNumeros.Infra.Interfaces.Polices
{
    public interface ICircuitBreakerPolicyProvider
    {
        AsyncCircuitBreakerPolicy GetPolicy();
    }
}
