using CrudTeste.Validations;
using System.ComponentModel.DataAnnotations;
using System;

namespace CrudTeste.DTOs
{
    public class EditarUsuarioDTO
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string EmailResgister { get; set; }

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
        public string Complemento { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        public string Telefone { get; set; }
    }
}
