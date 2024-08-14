using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CrudTeste.Validations
{
    public class CepValidation : ValidationAttribute
    {
        public CepValidation()
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;
            bool valido = ValidaCEP(value.ToString());
            return valido;
        }

        public bool ValidaCEP(string cep)

        {

            cep = cep.Replace(".", "");

            cep = cep.Replace("-", "");

            cep = cep.Replace(" ", "");



            Regex Rgx = new Regex(@"^\d{8}$");

            if (!Rgx.IsMatch(cep))

                return false;

            else

                return true;

        }
    }
}
