using MakingSolutions.Desenv.WebApi.Entities.Entities;

namespace MakingSolutions.Desenv.WebApi.Domain.Interfaces.InterfaceServices
{
    public interface IServiceMessage
    {
        Task<Message> AddMessage(Message Objeto);
        Task<Message> UpdateMessage(Message Objeto);
        Task<List<Message>> ListAtiveMessage();
    }
}
