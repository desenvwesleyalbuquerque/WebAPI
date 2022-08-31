using MakingSolutions.Desenv.WebApi.Domain.Infrastructure.Configuration;
using MakingSolutions.Desenv.WebApi.Domain.Infrastructure.Repository.Generics;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace MakingSolutions.Desenv.WebApi.Domain.Infrastructure.Repository.Repositories
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
