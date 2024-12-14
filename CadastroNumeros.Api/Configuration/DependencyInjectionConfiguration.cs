using CadastroNumeros.Domain.Configuration.IOptions;
using CadastroNumeros.Implementations;
using CadastroNumeros.Infra.Interfaces.Queues;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Infra.Queues.RabbitMQ;
using CadastroNumeros.Infra.Services;

namespace CadastroNumeros.Api.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ResolverDependencias(this IServiceCollection services, IConfiguration configuration)
    {
        //Injeção de dependência e banco devem ter a mesma duraçao (Scoped no caso)
        services.AddScoped(typeof(IContatoRepository), typeof(ContatoRepository));
        services.AddScoped(typeof(IContatoService), typeof(ContatoService));

        services.Configure<RabbitMQSetting>(configuration.GetSection("RabbitMQ"));
        services.AddScoped(typeof(IRabbitMQPublisher<>), typeof(RabbitMQPublisher<>));

        return services;
    }
}
