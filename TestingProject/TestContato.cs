using CadastroNumeros.Controllers;
using CadastroNumeros.Implementations;
using CadastroNumeros.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using Xunit;

namespace TestingProject
{
    public class TestContato
    {
        [Fact]
        public void Test_GetAll()
        {
            var contatoRepository = new Mock<IContatoRepository>();
            var contatoController = new ContatoController(contatoRepository.Object);
            var result = contatoController.GetAll();
            var actionRsult = Assert.IsType<ActionResult>(result);
            int count = result.Value.Count();
            Assert.Equal(0, count);
        }
    }
}
