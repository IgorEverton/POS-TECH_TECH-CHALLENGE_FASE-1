using CadastroNumeros.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CadastroNumeros.Teste.Models
{
    public class ContatoTests
    {
        private Contato _contato;

        public ContatoTests()
        {
            _contato = new Contato
            {
                Id = Guid.NewGuid(),
                DataCriacao = DateTime.Now,
                Nome = "João Silva",
                Idade = 30,
                Email = "joao.silva@example.com",
                Telefone = "123456789",
                CodigoDdd = 11
            };
        }

        [Fact]
        public void Contato_EValido()
        {
            var context = new ValidationContext(_contato, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_contato, context, results, true);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Contato_Nome_MaiorQueOLimite_DeveFalharAValidacao()
        {
            _contato.Nome = new string('a', 101);

            var context = new ValidationContext(_contato, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_contato, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, vr => vr.MemberNames.Contains(nameof(Contato.Nome)));
        }

        [Fact]
        public void Contato_Email_FormatoInvalido_DeveFalharAValidacao()
        {
            _contato.Email = "emailinvalido";

            var context = new ValidationContext(_contato, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_contato, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, vr => vr.MemberNames.Contains(nameof(Contato.Email)));
        }

        [Fact]
        public void Contato_Telefone_MaiorQueOLimite_DeveFalharAValidacao()
        {
            _contato.Telefone = new string('1', 10);

            var context = new ValidationContext(_contato, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_contato, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, vr => vr.MemberNames.Contains(nameof(Contato.Telefone)));
        }

        [Fact]
        public void Contato_CodigoDdd_Invalido_DeveFalharAValidacao()
        {
            // Presuming DddValidation attribute checks for valid DDD codes.
            _contato.CodigoDdd = 999;

            var context = new ValidationContext(_contato, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_contato, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, vr => vr.MemberNames.Contains(nameof(Contato.CodigoDdd)));
        }
    }
}