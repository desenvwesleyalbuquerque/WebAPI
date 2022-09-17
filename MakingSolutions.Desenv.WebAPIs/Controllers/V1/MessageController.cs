using AutoMapper;
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
    [Route("v{version:apiVersion}/api/[controller]")]
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

        [Produces("application/json")]
        [HttpPost, Route("AddMessage")]
        public async Task<List<Notifies>> AddMessage(MessageViewModel message)
        {
            message.UserId = await RetornarIdUsuarioLogado();
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.AddMessage(messageMap);
            return messageMap.Notificacoes;
        }


        [Produces("application/json")]
        [HttpPost, Route("UpdateMessage")]
        public async Task<List<Notifies>> Update(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.UpdateMessage(messageMap);
            return messageMap.Notificacoes;
        }


        [Produces("application/json")]
        [HttpPost, Route("DeleteMessage")]
        public async Task<List<Notifies>> DeleteMessage(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.DeleteMessage(messageMap);
            return messageMap.Notificacoes;
        }


        [Produces("application/json")]
        [HttpGet, Route("SearchMessageById")]
        public async Task<MessageViewModel> SearchMessageById(int id)
        {
            var message = await _IMessage.SearchMessageById(id);
            var messageMap = _IMapper.Map<MessageViewModel>(message);
            return messageMap;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost, Route("ListAll")]
        public async Task<List<MessageViewModel>> List()
        {
            var mensagens = await _IMessage.List();
            var messageMap = _IMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }


        [Obsolete("Método Obsolete")]
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost, Route("ListAtiveMessage")]
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

