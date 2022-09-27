using MakingSolutions.Desenv.WebApi.Entities.Entities;

namespace MakingSolutions.Desenv.WebAPIs.Models
{
    public class MessageViewModel
    {
        public Guid MessageId { get; set; }
        public string? Titulo { get; set; }
        public string? Body { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
