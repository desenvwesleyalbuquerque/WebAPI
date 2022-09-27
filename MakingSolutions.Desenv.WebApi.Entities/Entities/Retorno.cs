using System.ComponentModel.DataAnnotations.Schema;

namespace MakingSolutions.Desenv.WebApi.Entities.Entities
{
    public class Retorno
    {
        public Retorno(string mensagem = "")
        {
            Notifies = new Notifies();
            Mensagem = mensagem;
        }

        [NotMapped]
        public int code { get; set; }

        [NotMapped]
        public string Mensagem { get; set; }

        [NotMapped]
        public object ReturnData { get; set; }

        [NotMapped]
        public Notifies Notifies { get; set; }
    }
}
