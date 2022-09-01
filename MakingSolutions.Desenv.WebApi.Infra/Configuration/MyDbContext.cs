using MakingSolutions.Desenv.WebApi.Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MakingSolutions.Desenv.WebApi.Infra.Configuration
{

    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Message> Message { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ObterStringConexao());
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

            base.OnModelCreating(builder);
        }


        public string ObterStringConexao()
        {
            return "Server=(localdb)\\mssqllocaldb;Database=MakingSolutions;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

    }

}
