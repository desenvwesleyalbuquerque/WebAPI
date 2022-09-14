using AutoMapper;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using MakingSolutions.Desenv.WebApi.Infra.Configuration;
using MakingSolutions.Desenv.WebApi.Infra.Repository.Cache;
using MakingSolutions.Desenv.WebApi.Infra.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MakingSolutions.Desenv.WebApi.Infra.Repository.Repositories
{
    public class RepositoryMessage : RepositoryGenerics<Message>, IMessage
    {
        private readonly DbContextOptions<AppDbContext> _OptionsBuilder;
        //private readonly IConnectionMultiplexer _redis;
        //private readonly IDatabase _database;
        private readonly RedisClient _database;

        private readonly IMapper _IMapper;
        public RepositoryMessage()
        {
            _OptionsBuilder = new DbContextOptions<AppDbContext>();
            _database = new RedisClient();
        }

        public async Task AddMessage(Message message)
        {
            using (var context = new AppDbContext(_OptionsBuilder))
            {
                await context.Set<Message>().AddAsync(message);
                await context.SaveChangesAsync();
            }
            await UpdateMessageCacheAsync(message);
        }

        public async Task UpdateMessage(Message message)
        {
            using (var context = new AppDbContext(_OptionsBuilder))
            {
                context.Set<Message>().Update(message);
                await context.SaveChangesAsync();
            }
            await UpdateMessageCacheAsync(message);
        }

        public async Task<Message> SearchMessageById(int messageId)
        {
            Message? message = await GetMessageCacheAsync(messageId);
            if (message != null)
            {
                return message;
            }

            using (var context = new AppDbContext(_OptionsBuilder))
            {
                message = await context.Message.AsNoTracking().FirstOrDefaultAsync(m => m.MessageId.Equals(messageId));
                if (message != null)
                {
                    await UpdateMessageCacheAsync(message);
                }
                return message;
            }
        }

        public async Task<List<Message>> ListMessage(Expression<Func<Message, bool>> exMessage)
        {
            using (var context = new AppDbContext(_OptionsBuilder))
            {
                return await context.Message.Where(exMessage).AsNoTracking().ToListAsync();
            }
        }

        public async Task DeleteMessage(Message message)
        {
            using (var context = new AppDbContext(_OptionsBuilder))
            {
                var messageRemove = await context.Message.AsNoTracking().FirstOrDefaultAsync(m => m.MessageId.Equals(message.MessageId));
                if (messageRemove != null)
                {
                    context.Set<Message>().Remove(messageRemove);
                    await DeleteMessageCacheAsync(messageRemove);
                }
            }
        }

        public async Task<Message> GetMessageCacheAsync(int messageId)
        {
            Message message = null;

            string value = await _database.StringGetAsync(messageId.ToString());
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return
                JsonConvert.DeserializeObject<Message>(value, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        public async Task UpdateMessageCacheAsync(Message message)
        {
            await _database.StringSetAsync(message.MessageId.ToString(), JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));

        }

        public async Task DeleteMessageCacheAsync(Message message)
        {
            await _database.KeyDeleteAsync(message.MessageId.ToString());

        }

    }
}
