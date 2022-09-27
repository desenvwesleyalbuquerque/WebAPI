using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using MakingSolutions.Desenv.WebApi.Infra.Configuration;
using MakingSolutions.Desenv.WebApi.Infra.Repository.Cache;
using MakingSolutions.Desenv.WebApi.Infra.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace MakingSolutions.Desenv.WebApi.Infra.Repository.Repositories
{
    public class RepositoryMessage : RepositoryGenerics<Message>, IMessage
    {
        private readonly DbContextOptions<AppDbContext> _OptionsBuilder;
        private readonly RedisClient _database;

        public RepositoryMessage()
        {
            _OptionsBuilder = new DbContextOptions<AppDbContext>();
            _database = new RedisClient();
        }

        public async Task<Message> AddMessage(Message message)
        {
            using (var context = new AppDbContext(_OptionsBuilder))
            {
                await context.Set<Message>().AddAsync(message);
                await context.SaveChangesAsync();
            }
            await UpdateMessageCacheAsync(message);

            return message;
        }

        public async Task<Message> UpdateMessage(Message message)
        {
            using (var context = new AppDbContext(_OptionsBuilder))
            {
                context.Set<Message>().Update(message);
                await context.SaveChangesAsync();
            }
            await UpdateMessageCacheAsync(message);

            return message;
        }

        public async Task<Message> SearchMessageById(string messageId)
        {
            Message? message = await GetMessageCacheAsync(messageId);
            if (message != null)
            {
                return message;
            }

            using (var context = new AppDbContext(_OptionsBuilder))
            {
                message = await context.Message.FirstOrDefaultAsync(m => m.MessageId.ToString().Equals(messageId));
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
                return await context.Message.Include(x => x.ApplicationUser).Where(exMessage).AsNoTracking().OrderByDescending(x => x.DataCadastro).ToListAsync();
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

        public async Task<Message> GetMessageCacheAsync(string messageId)
        {
            Message? message = null;

            string value = await _database.StringGetAsync(messageId);
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
