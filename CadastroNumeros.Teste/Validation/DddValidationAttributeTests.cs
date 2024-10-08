using CadastroNumeros.Domain.Validation;

namespace CadastroNumeros.Teste.Validation
{
    public class DddValidationAttributeTests
    {
        private DddValidationAttribute _dddValidation;

        public DddValidationAttributeTests()
        {
            _dddValidation = new DddValidationAttribute();
        }

        [Theory]
        [InlineData(11)]
        [InlineData(21)]
        [InlineData(31)]
        [InlineData(41)]
        [InlineData(51)]
        [InlineData(61)]
        [InlineData(71)]
        [InlineData(81)]
        [InlineData(91)]
        public void IsValid_DDDValido_DeveRetornarTrue(int ddd)
        {
            var result = _dddValidation.IsValid(ddd);
            Assert.True(result);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        [InlineData(40)]
        [InlineData(50)]
        [InlineData(60)]
        [InlineData(70)]
        [InlineData(80)]
        [InlineData(90)]
        [InlineData(100)]
        [InlineData(110)]
        public void IsValid_DDDInvalido_DeveRetornarFalse(int ddd)
        {
            var result = _dddValidation.IsValid(ddd);
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ValorNulo_DeveRetornarFalse()
        {
            var result = _dddValidation.IsValid(null);
            Assert.False(result);
        }

        [Fact]
        public void IsValid_NaoInteiro_DeveRetornarFalse()
        {
            var result = _dddValidation.IsValid("NaoInteiro");
            Assert.False(result);
        }
    }
}