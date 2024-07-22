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

        private List<DDD> CreateInvalidsDDDs(Type type)
        {
            return GetDDDStub(type).GetInvalidDDDs();
        }

        private DDDstub GetDDDStub(Type type) => Activator.CreateInstance(type) as DDDstub;

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
            var dddList = CreateInvalidsDDDs(type);
            var stub = GetDDDStub(type);

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
