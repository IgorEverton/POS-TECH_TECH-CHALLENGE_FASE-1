using CadastroNumeros.Implementations;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Infra.Services;

namespace CadastroNumeros.Api.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ResolverDependencias(this IServiceCollection services)
    {
        //Injeção de dependência e banco devem ter a mesma duraçao (Scoped no caso)
        services.AddScoped(typeof(IContatoRepository), typeof(ContatoRepository));
        services.AddScoped(typeof(IContatoService), typeof(ContatoService));

        return services;
    }
}
