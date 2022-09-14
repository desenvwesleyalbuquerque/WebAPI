using MakingSolutions.Desenv.WebApi.Domain.Interfaces.Generics;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using System.Linq.Expressions;

namespace MakingSolutions.Desenv.WebApi.Domain.Interfaces
{
    public interface IMessage : IGeneric<Message>
    {
        Task AddMessage(Message message);
        Task<List<Message>> ListMessage(Expression<Func<Message, bool>> exMessage);
        Task<Message> SearchMessageById(int Id);
        Task UpdateMessage(Message message);
        Task DeleteMessage(Message message);
    }
}
