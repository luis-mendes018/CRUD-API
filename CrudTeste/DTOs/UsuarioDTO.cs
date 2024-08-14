using CrudTeste.Validations;
using System.ComponentModel.DataAnnotations;

namespace CrudTeste.DTOs
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "Informe o nome de usuário")]
        [Display(Name = "Usuário")]
        [StringLength(50, ErrorMessage = "Limite de caracteres excedido!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Informe a senha!")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [SenhaValidation(ErrorMessage = "Formato de senha inválido")]
        public string Password { get; set; }
    }
}
