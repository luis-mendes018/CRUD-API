using System.ComponentModel.DataAnnotations;

namespace CrudTeste.Validations
{
    public class SenhaValidation : ValidationAttribute
    {
        public int MinLength { get; set; } = 6;
        public int MaxLength { get; set; } = 20;
        public bool RequireDigit { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireSpecialCharacter { get; set; } = true;

        public override bool IsValid(object value)
        {
            string password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return true;
            }

            if (password.Length < MinLength || password.Length > MaxLength)
            {
                return false;
            }

            bool hasDigit = false;
            bool hasLowercase = false;
            bool hasUppercase = false;
            bool hasSpecialCharacter = false;

            foreach (char c in password)
            {
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
                else if (char.IsLower(c))
                {
                    hasLowercase = true;
                }
                else if (char.IsUpper(c))
                {
                    hasUppercase = true;
                }
                else if (!char.IsLetterOrDigit(c))
                {
                    hasSpecialCharacter = true;
                }
            }

            return (!RequireDigit || hasDigit)
                && (!RequireLowercase || hasLowercase)
                && (!RequireUppercase || hasUppercase)
                && (!RequireSpecialCharacter || hasSpecialCharacter);
        }
    }
}
