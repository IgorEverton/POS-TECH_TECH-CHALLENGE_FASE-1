using CadastroNumeros.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroNumeros.Teste.Data
{
    public static class DbOptionsCreator
    {
        public static DbContextOptions<AppDbContext> CreateDbOptions()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var testesEmMemoria = configuration.GetValue<bool>("TestesEmMemoria");

            if(testesEmMemoria is true)
            {
                return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnectionTesteIntegracao");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Não foi encontrada nenhuma DefaultConnectionTesteIntegracao");
            }

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return options;
        }
    }
}
