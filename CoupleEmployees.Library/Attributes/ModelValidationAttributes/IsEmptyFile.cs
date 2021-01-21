using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoupleEmployees.Library.Attributes.ModelValidationAttributes
{
    public class IsEmptyFile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult("Please select a file!", new List<string>() { "TextFile" });
            }

            return ValidationResult.Success;
        }
    }
}
