using System.Net;
using System.Text.RegularExpressions;

namespace CadastroNumeros.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Endereco { get; } = string.Empty;
        public static implicit operator string(Email email) => email.Endereco;
        public static implicit operator Email(string endereco) => new(endereco);
        public override string ToString() => Endereco;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Endereco;
        }

        public Email() { }

        public Email(string endereco)
        {
            if (string.IsNullOrEmpty(endereco) || endereco.Length < 5)
                throw new Exception("Email inválido");

            Endereco = endereco.ToLower().Trim();
            const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            if (!Regex.IsMatch(endereco, pattern))
                throw new Exception("Email inválido");

            Endereco = endereco;
        }
    }
}
