using MakingSolutions.Desenv.WebApi.Entities.Entities;

namespace MakingSolutions.Desenv.WebApi.Domain.Interfaces.InterfaceServices
{
    public interface IServiceMessage
    {
        Task Adicionar(Message Objeto);
        Task Atualizar(Message Objeto);
        Task<List<Message>> ListarMensagensAtiva();
    }
}
