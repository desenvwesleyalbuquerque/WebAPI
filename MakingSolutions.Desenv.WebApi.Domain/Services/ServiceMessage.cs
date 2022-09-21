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

        public async Task AddMessage(Message Objeto)
        {
            var validaTitulo = Objeto.ValidaPropiedadeString(Objeto.Titulo, "Título");
            if (validaTitulo)
            {
                Objeto.Ativo = true;
                Objeto.DataCadastro = DateTime.Now;              
                await _IMessage.AddMessage(Objeto);
            }
        }

        public async Task UpdateMessage(Message Objeto)
        {
            var validaTitulo = Objeto.ValidaPropiedadeString(Objeto.Titulo, "Título");
            if (validaTitulo)
            {
                var messageEdit = await _IMessage.SearchMessageById(Objeto.MessageId);
                if (messageEdit is null)
                {
                    throw new InvalidOperationException("Mensagem não localizada na base de dados!");
                }

                messageEdit.Titulo = Objeto.Titulo;
                messageEdit.Ativo = Objeto.Ativo;
                messageEdit.DataAlteracao = DateTime.Now;

                await _IMessage.UpdateMessage(Objeto);
            }
        }

        public async Task<List<Message>> ListAtiveMessage()
        {
            return await _IMessage.ListMessage(x => x.Ativo);
        }
    }
}
