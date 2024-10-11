using CadastroNumeros.Domain.Models;
using CadastroNumeros.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace CadastroNumeros.Teste.Models
{
    public class ContatoTests
    {
        private Contato _contato;

        public ContatoTests()
        {
            var email = new Email("joao.silva@example.com");

            _contato = new Contato
            (
                Guid.NewGuid(),
                DateTime.Now,
                "João Silva",
                30,
                "123456789",
                email,
                11
            );
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
            _contato.SetNome(new string('a', 101));

            var context = new ValidationContext(_contato, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_contato, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, vr => vr.MemberNames.Contains(nameof(Contato.Nome)));
        }

        [Fact]
        public void Contato_Email_FormatoInvalido_DeveFalharAValidacao()
        {
            _contato.SetEmail("emailinvalido");

            var context = new ValidationContext(_contato, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_contato, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, vr => vr.MemberNames.Contains(nameof(Contato.Email)));
        }

        [Fact]
        public void Contato_Telefone_MaiorQueOLimite_DeveFalharAValidacao()
        {
            var telefone = new string('1', 10);
            _contato.SetTelefone(telefone);

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
            _contato.SetCodigoDdd(999);

            var context = new ValidationContext(_contato, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_contato, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, vr => vr.MemberNames.Contains(nameof(Contato.CodigoDdd)));
        }
    }
}