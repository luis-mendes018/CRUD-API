﻿using System;
using System.ComponentModel.DataAnnotations;

using CrudTeste.Validations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CrudTeste.Models
{
    public class ApplicationUser : IdentityUser
    {
        public override string Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [RgValidation(ErrorMessage = "RG inválido!")]
        public string Rg { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(50, ErrorMessage = "Limite de caracteres excedido!")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "CEP obrigatório!")]
        [CepValidation(ErrorMessage = "Cep inválido")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Digite o endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Digite o bairro")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Digite o número")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "Insira o nome do estado!")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Insira o nome da cidade!")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "CPF obrigatório!")]
        [CpfValidation(ErrorMessage = "CPF inválido!")]
        public string CPF { get; set; }

        public string Complemento { get; set; }

        [Required(ErrorMessage = "Informe a senha!")]
        [DataType(DataType.Password)]
        [SenhaValidation(ErrorMessage = "Senha requer entre 6 e 20 caracteres!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirme sua senha")]
        [DataType(DataType.Password)]
        [SenhaValidation(ErrorMessage = "Senha requer entre 6 e 20 caracteres!")]
        [Compare(nameof(Password), ErrorMessage = "A senha e a confirmação não são iguais")]
        public string PasswordConfirm { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public byte[] ProfileImage { get; set; }
    }
}
