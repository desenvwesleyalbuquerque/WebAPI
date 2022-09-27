using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces.InterfaceServices;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using MakingSolutions.Desenv.WebAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakingSolutions.Desenv.WebAPIs.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/api/Message")]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _IMapper;
        private readonly IMessage _IMessage;
        private readonly IServiceMessage _IServiceMessage;

        public MessageController(IMapper IMapper,
                                 IMessage IMessage,
                                 IServiceMessage IServiceMessage)
        {
            _IMapper = IMapper;
            _IMessage = IMessage;
            _IServiceMessage = IServiceMessage;
        }

        /// <summary>
        /// Adicionar novo modelo de mensagem
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost, Route("AddMessage")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Retorno> AddMessage(
            [FromBody] MessageAddViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            messageMap.UserId = await RetornarIdUsuarioLogado();
            var mensagem = await _IServiceMessage.AddMessage(messageMap);

            return new Retorno()
            {
                ReturnData = _IMapper.Map<MessageViewModel>(mensagem),
                Notifies = mensagem.Notifies
            };
        }


        [HttpPut, Route("UpdateMessage")]
        public async Task<Retorno> Update(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            var mensagem = await _IServiceMessage.UpdateMessage(messageMap);

            return new Retorno()
            {
                ReturnData = _IMapper.Map<MessageViewModel>(mensagem),
                Notifies = mensagem.Notifies
            };
        }

        /// <summary>
        /// Deletes a specific .
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>        
        [HttpDelete, Route("DeleteMessage")]
        [Produces("application/json")]
        public async Task<Retorno> DeleteMessage(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.DeleteMessage(messageMap);
            return new Retorno("Removido com sucesso!");
        }


        [HttpGet, Route("SearchMessageById")]
        public async Task<Retorno> SearchMessageById(string messageId)
        {
            var message = await _IMessage.SearchMessageById(messageId);

            if (message is null)
                return new Retorno();

            return new Retorno()
            {
                ReturnData = _IMapper.Map<MessageViewModel>(message),
                Notifies = message.Notifies
            };
        }

        [HttpGet, Route("ListAll")]
        public async Task<List<MessageViewModel>> List()
        {
            var mensagens = await _IServiceMessage.ListAtiveMessage();
            var messageMap = _IMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }


        [Obsolete("Método Obsolete")]
        [Produces("application/json")]
        [HttpGet, Route("ListAtiveMessage")]
        public async Task<List<MessageViewModel>> ListAtiveMessage()
        {
            var mensagens = await _IServiceMessage.ListAtiveMessage();
            var messageMap = _IMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }

        private async Task<string> RetornarIdUsuarioLogado()
        {
            if (User != null)
            {
                var idUsuario = User.FindFirst("idUsuario");
                return idUsuario.Value;
            }

            return string.Empty;
        }
    }

}

