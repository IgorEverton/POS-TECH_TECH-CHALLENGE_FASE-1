using CadastroNumeros.Domain.Models;

namespace CadastroNumeros.Teste.Stubs
{
    public class ContatoStub
    {
        public Contato Get()
        {
            Contato contato = new()
            {
                DataCriacao = DateTime.Now,
                Idade = 22,
                Nome = "teste",
                CodigoDdd = 84,
                Email = "teste@email.com",
                Id = Guid.NewGuid(),
                Telefone = "123456789"
            };

            return contato;
        }

        public List<Contato> Get(int quantidade)
        {
            var lista = new List<Contato>();

            for (var q = 0; q < quantidade; ++q)
            {
                lista.Add(
                    new()
                    {
                        DataCriacao = DateTime.Now,
                        Idade = int.Parse($"2{q}"),
                        Nome = $"teste{q}",
                        CodigoDdd = int.Parse($"8{q}"),
                        Email = $"teste{q}@email.com",
                        Id = Guid.NewGuid(),
                        Telefone = $"12345678{q}"
                    }
                );
            }

            return lista;
        }

        //private readonly HashSet<string> DDDsValidos =
        //[
        //    "11", "12", "13", "14", "15", "16", "17", "18", "19",
        //    "21", "22", "24", "27", "28", "31", "32", "33", "34", "35", "37", "38", "41", "42", "43", "44", "45", "46",
        //    "47", "48", "49", "51", "53", "54", "55", "61", "62", "63", "64", "65", "66", "67", "68", "69",
        //    "71", "73", "74", "75", "77", "79", "81", "82", "83", "84", "85", "86", "87", "88", "89", "91", "92", "93", "94", "95", "96", "97", "98", "99"
        //];

        //public bool IsValid(string Codigo)
        //{
        //    return DDDsValidos.Contains(Codigo);
        //}
    }
}
