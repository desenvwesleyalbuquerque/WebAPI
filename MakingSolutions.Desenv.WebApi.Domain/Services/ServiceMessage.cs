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

        public async Task Adicionar(Message Objeto)
        {
            var validaTitulo = Objeto.ValidaPropiedadeString(Objeto.Titulo, "Título");
            if (validaTitulo)
            {
                Objeto.DataCadastro = DateTime.Now;
                Objeto.DataAlteracao = DateTime.Now;
                Objeto.Ativo = true;
                await _IMessage.Add(Objeto);
            }
        }

        public async Task Atualizar(Message Objeto)
        {
            var validaTitulo = Objeto.ValidaPropiedadeString(Objeto.Titulo, "Título");
            if (validaTitulo)
            {
                Objeto.DataAlteracao = DateTime.Now;
                await _IMessage.Update(Objeto);
            }
        }

        public async Task<List<Message>> ListarMensagensAtiva()
        {
            return await _IMessage.ListarMessage(x => x.Ativo);
        }
    }
}
