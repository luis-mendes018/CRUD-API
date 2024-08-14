using System;

using Microsoft.AspNetCore.Http;

namespace CrudTeste.DTOs
{
    public class UsuariosDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string EmailRegister { get; set; }
        public string Rg { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sobrenome { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public int Numero { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public string Telefone { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
