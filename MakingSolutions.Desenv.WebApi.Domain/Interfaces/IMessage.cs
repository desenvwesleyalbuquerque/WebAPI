using MakingSolutions.Desenv.WebApi.Domain.Interfaces.Generics;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using System.Linq.Expressions;

namespace MakingSolutions.Desenv.WebApi.Domain.Interfaces
{
    public interface IMessage : IGeneric<Message>
    {
        Task<Message> AddMessage(Message message);
        Task<List<Message>> ListMessage(Expression<Func<Message, bool>> exMessage);
        Task<Message> SearchMessageById(string Id);
        Task<Message> UpdateMessage(Message message);
        Task DeleteMessage(Message message);
    }
}
