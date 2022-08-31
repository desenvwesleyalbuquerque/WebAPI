using System.ComponentModel.DataAnnotations.Schema;

namespace MakingSolutions.Desenv.WebApi.Entities.Entities
{
    public class Notifies
    {
        public Notifies()
        {
            Notificacoes = new List<Notifies>();
        }

        [NotMapped]
        public string NomePropiedade { get; set; }

        [NotMapped]
        public string Mensagem { get; set; }

        [NotMapped]
        public List<Notifies> Notificacoes { get; set; }


        public bool ValidaPropiedadeString(string valor, string nomePropiedade)
        {
            if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrWhiteSpace(nomePropiedade))
            {
                Notificacoes.Add(new Notifies()
                {
                    Mensagem = "Campo Obrigatório",
                    NomePropiedade = nomePropiedade

                });

                return false;
            }

            return true;
        }

        public bool ValidaPropiedadeInt(int valor, string nomePropiedade)
        {
            if (valor < 1 || string.IsNullOrWhiteSpace(nomePropiedade))
            {
                Notificacoes.Add(new Notifies()
                {
                    Mensagem = "Campo Obrigatório",
                    NomePropiedade = nomePropiedade

                });

                return false;
            }

            return true;
        }
    }
}
