using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using MakingSolutions.Desenv.WebApi.Infra.Configuration;
using MakingSolutions.Desenv.WebApi.Infra.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSolutions.Desenv.WebApi.Infra.Repository.Repositories
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
