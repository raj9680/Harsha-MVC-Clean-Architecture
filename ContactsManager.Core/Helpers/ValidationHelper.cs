using System.ComponentModel.DataAnnotations;

namespace Service.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            // List to store validation errors
            List<ValidationResult> validationResult = new List<ValidationResult>();
            // even if any one model is found invalid it return false
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResult.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
