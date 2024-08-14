using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrudTeste.Validations
{
    public class CpfValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var cpf = value as string;

            if (string.IsNullOrEmpty(cpf))
            {
                return new ValidationResult("CPF é obrigatório.");
            }

            if (!IsValidCPF(cpf))
            {
                return new ValidationResult("CPF inválido.");
            }

            return ValidationResult.Success;
        }

        public bool IsValidCPF(string cpf)
        {
            // Remove caracteres não numéricos (pontos e traços)
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
                return false;

            var tempCpf = cpf.Substring(0, 9);
            var sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * (10 - i);

            var remainder = sum % 11;
            var firstDigit = remainder < 2 ? 0 : 11 - remainder;

            tempCpf += firstDigit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            var secondDigit = remainder < 2 ? 0 : 11 - remainder;

            return cpf.EndsWith($"{firstDigit}{secondDigit}");
        }
    }
}
