using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using MakingSolutions.Desenv.WebApi.Infrastructure.Configuration;
using MakingSolutions.Desenv.WebApi.Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;

namespace MakingSolutions.Desenv.WebApi.Infrastructure.Repository.Repositories
{
    public class RepositoryMessage : RepositoryGenerics<Message>, IMessage
    {
        private readonly DbContextOptions<MyDbContext> _OptionsBuilder;

        public RepositoryMessage()
        {
            _OptionsBuilder = new DbContextOptions<MyDbContext>();
        }
    }
}
