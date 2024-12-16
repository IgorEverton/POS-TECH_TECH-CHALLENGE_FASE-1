using CadastroNumeros.Contato.Consumer.Consumers;
using CadastroNumeros.Domain.Configuration.IOptions;
using CadastroNumeros.Implementations;
using CadastroNumeros.Infra.Data;
using CadastroNumeros.Infra.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Iniciando consumer!");

var builder = Host.CreateApplicationBuilder();

var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

var connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddScoped<IContatoRepository, ContatoRepository>();

builder.Services.AddHostedService<ContatoMessageConsumerService>();

var host = builder.Build();

host.Run();
