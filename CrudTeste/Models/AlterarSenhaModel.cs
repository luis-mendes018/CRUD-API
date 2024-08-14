using CrudTeste.Validations;
using System.ComponentModel.DataAnnotations;

namespace CrudTeste.Models
{
    public class AlterarSenhaModel
    {
        [Required(ErrorMessage = "Informe sua senha atual")]
        [DataType(DataType.Password)]
        [SenhaValidation(ErrorMessage = "Formato de senha inválido")]
        public string PasswordNow { get; set; }

        [Required(ErrorMessage = "Informe sua nova senha")]
        [DataType(DataType.Password)]
        [SenhaValidation(ErrorMessage = "Formato de senha inválido")]
        public string PasswordNew { get; set; }

        [Required(ErrorMessage = "Confirme sua nova senha")]
        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "A senha e a confirmação" + " não estão iguais")]
        [SenhaValidation(ErrorMessage = "Formato de senha inválido")]
        public string PasswordNewConfirm { get; set; }
    }
}
