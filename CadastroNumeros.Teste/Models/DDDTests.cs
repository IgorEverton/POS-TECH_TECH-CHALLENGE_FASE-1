using CadastroNumeros.Domain.Models;
using CadastroNumeros.Implementations;
using CadastroNumeros.Teste.Stubs;
using Moq;
using System;
using System.Collections;
using Xunit;

namespace CadastroNumeros.Teste.Models
{
    public class DDDTests
    {
        private MockRepository mockRepository;



        public DDDTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private DDD CreateDDD()
        {
            return new DDD();
        }

        [Fact]
        public void ToString_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dDD = this.CreateDDD();

            // Act
            var result = dDD.ToString();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(typeof(DDDstub))]
        public void ToString_StateUnderTest_ErrorBehavior(Type type)
        {
            // Arrange
            var stub = Activator.CreateInstance(type) as DDDstub;
            var dddList = stub.GetInvalidDDDs();

            // Act
            foreach (var ddd in dddList)
            { 
                var result = stub.IsValid(ddd.Codigo.ToString());
                // Assert
                Assert.False(result);
            }
        }
    }
}
