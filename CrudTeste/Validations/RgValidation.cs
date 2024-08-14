using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CrudTeste.Validations
{
    public class RgValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rg = value as string;

            // Verifica se o RG está vazio
            if (string.IsNullOrEmpty(rg))
            {
                return new ValidationResult("RG é obrigatório.");
            }

            // Define o padrão de regex para RG (simplesmente, números com ou sem pontos e traços)
            var regex = new Regex(@"^\d{2}\.?\d{3}\.?\d{3}-?\d{1}$");

            // Verifica se o RG corresponde ao padrão
            if (!regex.IsMatch(rg))
            {
                return new ValidationResult("RG inválido.");
            }

            return ValidationResult.Success;
        }
    }
}
