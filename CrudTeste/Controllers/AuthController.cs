using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using CrudTeste.DTOs;
using CrudTeste.Models;
using CrudTeste.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CrudTeste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }



        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController  :: Acessado em : "
                + DateTime.Now.ToLongDateString();
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromForm] CadastroUsuarioDTO user)
        {
            if (user.Password != user.PasswordConfirm)
            {
                return BadRequest("A senha e a confirmação de senha não correspondem.");
            }

            // Normaliza o CPF removendo pontos e traços
            var normalizedCPF = user.CPF.Replace(".", "").Replace("-", "");

            var cpfExists = await _userManager.Users.FirstOrDefaultAsync(u => u.CPF == normalizedCPF);
            if (cpfExists != null)
            {
                return BadRequest("Esse CPF já está em uso.");
            }

            byte[]? profileImageBytes = null;

            // Verifica se a imagem foi fornecida
            if (user.ProfileImage != null && user.ProfileImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    // Copia o conteúdo do arquivo para o MemoryStream
                    await user.ProfileImage.CopyToAsync(ms);

                    // Converte o MemoryStream para um array de bytes
                    profileImageBytes = ms.ToArray();
                }
            }

            ApplicationUser usuario = new ApplicationUser
            {
                Email = user.EmailResgister,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = user.UserName,
                CPF = normalizedCPF,
                Endereco = user.Endereco,
                Bairro = user.Bairro,
                Cidade = user.Cidade,
                Estado = user.Estado,
                Rg = user.Rg,
                DataNascimento = user.DataNascimento,
                Complemento = user.Complemento,
                Numero = user.Numero,
                PhoneNumber = user.Telefone,
                Cep = user.Cep,
                Password = user.Password,
                PasswordConfirm = user.PasswordConfirm,
                Sobrenome = user.Sobrenome,
                ProfileImage = profileImageBytes // Aqui você está atribuindo o array de bytes
            };

            var result = await _userManager.CreateAsync(usuario, user.Password!);
            if (!result.Succeeded)
            {
                return BadRequest("Falha ao criar o usuário");
            }

            return Ok("Usuário criado com sucesso!");
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                

                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("IsAuthenticated", "true")
        };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }

            return Unauthorized("Credenciais incorretas!");
        }




        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenModel.AccessToken
                ?? throw new ArgumentNullException(nameof(tokenModel));
            string refreshToken = tokenModel.RefreshToken
                ?? throw new ArgumentException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);
            if (principal == null)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username!);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });

        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return BadRequest("Inalid user name");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost("MudarSenha")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword(AlterarSenhaModel model)
        {
            // Obtém o nome de usuário do usuário atualmente logado
            var username = User.Identity.Name;
            if (username == null)
            {
                return BadRequest("Nome de usuário não encontrado na solicitação.");
            }

            // Obtém o objeto de usuário correspondente ao nome de usuário
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }


            if (model.PasswordNow == model.PasswordNew)
            {
                return BadRequest("A nova senha não pode ser igual a senha atual.");
            }

            // Verifica se a senha atual está correta
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.PasswordNow);
            if (!passwordCheck)
            {
                return BadRequest("A senha atual está incorreta.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.PasswordNow, model.PasswordNew);
            if (result.Succeeded)
            {
                return Ok("Senha alterada com sucesso!");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
