using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Couple_Employees.Attributes.ModelValidationAttributes
{
    public class Accept_Only_Txt_Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!((IFormFile)value).FileName.EndsWith(".txt"))
            {
                return new ValidationResult("You can upload only text files with extension .txt!", new List<string>() { "TextFile" });
            }

            return ValidationResult.Success;
        }
    }
}
