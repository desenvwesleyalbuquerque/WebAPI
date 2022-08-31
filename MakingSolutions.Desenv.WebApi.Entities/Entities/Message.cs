using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakingSolutions.Desenv.WebApi.Entities.Entities
{
    [Table("Message")]
    public class Message : Notifies
    {
        [Column("MessageId")]
        public int MessageId { get; set; }

        [Column("Titulo")]
        [MaxLength(255)]
        public string Titulo { get; set; }

        [Column("Ativo")]
        public bool Ativo { get; set; }

        [Column("DataCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DataAlteracao")]
        public DateTime DataAlteracao { get; set; }

        [ForeignKey("ApplicationUser")]
        [Column(Order = 1)]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
