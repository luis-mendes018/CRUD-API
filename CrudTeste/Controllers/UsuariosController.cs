using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using CrudTeste.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using PagedList;
using CrudTeste.DTOs;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace CrudTeste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        // Obter todos os usuários
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(user => new UsuariosDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    EmailRegister = user.Email,
                    Rg = user.Rg,
                    DataNascimento = user.DataNascimento,
                    Sobrenome = user.Sobrenome,
                    Cep = user.Cep,
                    Endereco = user.Endereco,
                    Bairro = user.Bairro,
                    Numero = user.Numero,
                    Estado = user.Estado,
                    Cidade = user.Cidade,
                    Complemento = user.Complemento,
                    Telefone = user.PhoneNumber
                })
                .ToListAsync();

            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }


            var usuarioDTO = new UsuariosDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                EmailRegister = user.Email,
                Rg = user.Rg,
                DataNascimento = user.DataNascimento,
                Sobrenome = user.Sobrenome,
                Cep = user.Cep,
                Endereco = user.Endereco,
                Bairro = user.Bairro,
                Numero = user.Numero,
                Estado = user.Estado,
                Cidade = user.Cidade,
                Complemento = user.Complemento,
                Telefone = user.PhoneNumber
            };

            return Ok(usuarioDTO);
        }


        // Criar um novo usuário
        [HttpPost("CriarUsuario")]
        public async Task<IActionResult> CreateUser([FromBody] CadastroUsuarioDTO userDTO)
        {
            if (userDTO.Password != userDTO.PasswordConfirm)
            {
                return BadRequest("A senha e a confirmação de senha não correspondem.");
            }

            // Normaliza o CPF removendo pontos e traços
            var normalizedCPF = userDTO.CPF.Replace(".", "").Replace("-", "");

            var cpfExists = await _userManager.Users.FirstOrDefaultAsync(u => u.CPF == normalizedCPF);
            if (cpfExists != null)
            {
                return BadRequest("Esse CPF já está em uso.");
            }

            var user = new ApplicationUser
            {
               Email = userDTO.EmailResgister,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userDTO.UserName,
                CPF = normalizedCPF,
                Endereco = userDTO.Endereco,
                Bairro = userDTO.Bairro,
                Cidade = userDTO.Cidade,
                Estado = userDTO.Estado,
                Rg = userDTO.Rg,
                DataNascimento = userDTO.DataNascimento,
                Complemento = userDTO.Complemento,
                Numero = userDTO.Numero,
                PhoneNumber = userDTO.Telefone,
                Cep = userDTO.Cep,
                Password = userDTO.Password,
                PasswordConfirm = userDTO.PasswordConfirm,
                Sobrenome = userDTO.Sobrenome
            };

            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Usuário criado com sucesso!");
        }

        // Editar um usuário
        [HttpPut("EditarUsuario/{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody] EditarUsuarioDTO userDTO)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            user.UserName = userDTO.UserName ?? user.UserName;
            user.Email = userDTO.EmailResgister ?? user.Email;
            user.Rg = userDTO.Rg ?? user.Rg;
            user.DataNascimento = userDTO.DataNascimento != DateTime.MinValue ? userDTO.DataNascimento : user.DataNascimento;
            user.Sobrenome = userDTO.Sobrenome ?? user.Sobrenome;
            user.Cep = userDTO.Cep ?? user.Cep;
            user.Endereco = userDTO.Endereco ?? user.Endereco;
            user.Bairro = userDTO.Bairro ?? user.Bairro;
            user.Numero = userDTO.Numero != 0 ? userDTO.Numero : user.Numero;
            user.Estado = userDTO.Estado ?? user.Estado;
            user.Cidade = userDTO.Cidade ?? user.Cidade;
            user.Complemento = userDTO.Complemento ?? user.Complemento;
            user.PhoneNumber = userDTO.Telefone ?? user.PhoneNumber;


            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Usuário atualizado com sucesso!");
        }

        // Excluir um usuário
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Usuário excluído com sucesso!");
        }

    }
}
