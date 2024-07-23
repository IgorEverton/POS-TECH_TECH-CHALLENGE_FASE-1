using CadastroNumeros.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroNumeros.Teste.Stubs
{
    public class DDDstub
    {
        public List<DDD> GetInvalidDDDs()
        {
            var listaContato = new List<Contato>()
            {
                new()
                {
                    DDD = 1,
                    DataCriacao = DateTime.Now,
                    Endereco = "endereco de teste",
                    Id = 1,
                    Idade = 22,
                    Nome = "teste"
                }
            };

            var dddsList = new List<DDD>
            {
                new()
                {
                    Codigo = 0,
                    Regiao = "teste",
                    Contatos = listaContato
                },
                new()
                {
                    Codigo = 1,
                    Regiao = null,
                    Contatos = listaContato
                }
            };

            return dddsList;
        }

        private readonly HashSet<string> DDDsValidos =
        [
            "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "21", "22", "24", "27", "28", "31", "32", "33", "34", "35", "37", "38", "41", "42", "43", "44", "45", "46",
            "47", "48", "49", "51", "53", "54", "55", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "71", "73", "74", "75", "77", "79", "81", "82", "83", "84", "85", "86", "87", "88", "89", "91", "92", "93", "94", "95", "96", "97", "98", "99"
        ];

        public bool IsValid(string Codigo)
        {
            return DDDsValidos.Contains(Codigo);
        }
    }
}
