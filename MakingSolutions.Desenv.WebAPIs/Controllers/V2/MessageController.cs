using AutoMapper;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces.InterfaceServices;
using MakingSolutions.Desenv.WebAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakingSolutions.Desenv.WebAPIs.Controllers.V2
{
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _IMapper;
        private readonly IMessage _IMessage;

        public MessageController(IMapper IMapper,
                                 IMessage IMessage,
                                 IServiceMessage IServiceMessage)
        {
            _IMapper = IMapper;
            _IMessage = IMessage;
        }

        [Produces("application/json")]
        [HttpPost, Route("ListActiveMessage")]
        public async Task<List<MessageViewModel>> ListActiveMessage()
        {
            var mensagens = await _IMessage.ListMessage(x => x.Ativo == true);
            var messageMap = _IMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }

    }

}

