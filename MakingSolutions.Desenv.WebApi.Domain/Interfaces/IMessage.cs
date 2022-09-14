using MakingSolutions.Desenv.WebApi.Domain.Interfaces.Generics;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using System.Linq.Expressions;

namespace MakingSolutions.Desenv.WebApi.Domain.Interfaces
{
    public interface IMessage : IGeneric<Message>
    {
        Task<List<Message>> ListarMessage(Expression<Func<Message, bool>> exMessage);
        Task<Message> GetMessageById(int Id);
    }
}
