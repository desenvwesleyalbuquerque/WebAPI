using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces.InterfaceServices;
using MakingSolutions.Desenv.WebApi.Entities.Entities;

namespace MakingSolutions.Desenv.WebApi.Domain.Services
{
    public class ServiceMessage : IServiceMessage
    {
        private readonly IMessage _IMessage;

        public ServiceMessage(IMessage message)
        {
            _IMessage = message;
        }

        public async Task<Message> AddMessage(Message Objeto)
        {
            var validaTitulo = Objeto.Notifies.ValidaPropiedadeString(Objeto.Titulo ?? "", "Título");
            if (validaTitulo)
            {
                Objeto.Ativo = true;
                Objeto.DataCadastro = DateTime.Now;
                Objeto = await _IMessage.AddMessage(Objeto);
            }

            return Objeto;
        }

        public async Task<Message> UpdateMessage(Message Objeto)
        {
            var validaTitulo = Objeto.Notifies.ValidaPropiedadeString(Objeto.Titulo ?? "", "Título");
            if (validaTitulo)
            {
                var messageEdit = await _IMessage.SearchMessageById(Objeto.MessageId.ToString());
                if (messageEdit is null)
                {
                    throw new InvalidOperationException("Mensagem não localizada na base de dados!");
                }

                messageEdit.Titulo = Objeto.Titulo;
                messageEdit.Ativo = Objeto.Ativo;
                messageEdit.DataAlteracao = DateTime.Now;

                Objeto = await _IMessage.UpdateMessage(Objeto);
            }
            return Objeto;
        }

        public async Task<List<Message>> ListAtiveMessage()
        {
            return await _IMessage.ListMessage(x => x.Ativo);
        }
    }
}
