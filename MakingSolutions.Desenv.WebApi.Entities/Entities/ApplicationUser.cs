using MakingSolutions.Desenv.WebApi.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakingSolutions.Desenv.WebApi.Entities.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Column("Cpf")]
        public string? Cpf { get; set; }

        [Column("TipoUsuario")]
        public TipoUsuario? Tipo { get; set; }
    }
}
