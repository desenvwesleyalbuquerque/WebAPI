using MakingSolutions.Desenv.WebApi.Entities.Entities;

namespace MakingSolutions.Desenv.WebApi.Domain.Interfaces.InterfaceServices
{
    public interface IServiceMessage
    {
        Task AddMessage(Message Objeto);
        Task UpdateMessage(Message Objeto);
        Task<List<Message>> ListAtiveMessage();
    }
}
