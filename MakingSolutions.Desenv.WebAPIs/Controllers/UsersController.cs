using MakingSolutions.Desenv.WebApi.Entities.Entities;
using MakingSolutions.Desenv.WebApi.Entities.Enums;
using MakingSolutions.Desenv.WebAPIs.Models;
using MakingSolutions.Desenv.WebAPIs.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace MakingSolutions.Desenv.WebAPIs.Controllers
{
    [Route("v1/api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signManager
            )
        {
            _userManager = userManager;
            _signManager = signManager;

        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("CriarTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            {
                return Unauthorized();
            }

            var resultado = await
                _signManager.PasswordSignInAsync(login.email, login.senha, false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                // Recupera Usuário Logado
                var userCurrent = await _userManager.FindByEmailAsync(login.email);
                var idUsuario = userCurrent.Id;

                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("MakingSolutions")
                .AddIssuer("MakingSolutions.Securiry.Bearer")
                .AddAudience("MakingSolutions.Securiry.Bearer")
                .AddClaim("idUsuario", idUsuario)
                //.AddExpiry(720)
                .Builder();

                var userLog = new
                {
                    idUsuario = userCurrent.Id,
                    userCurrent.UserName,
                    userCurrent.Cpf,
                    userCurrent.Email,
                    userCurrent.EmailConfirmed,
                    token = new
                    {
                        token.ValidTo,
                        token = token.value
                    }
                };

                return Ok(userLog);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("AdicionaUsuarioIdentity")]
        public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
                return Ok("Falta alguns dados");


            var user = new ApplicationUser
            {
                UserName = login.email,
                Email = login.email,
                Cpf = login.cpf,
                Tipo = TipoUsuario.Comun,
            };

            var resultado = await _userManager.CreateAsync(user, login.senha);

            if (resultado.Errors.Any())
            {
                return Ok(resultado.Errors);
            }


            // Geração de Confirmação caso precise
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email 
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

            if (resultado2.Succeeded)
                return Ok("Usuário Adicionado com Sucesso");
            else
                return Ok("Erro ao confirmar usuários");

        }
    }
}
