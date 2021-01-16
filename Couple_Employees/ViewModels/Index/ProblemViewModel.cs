using Couple_Employees.Attributes.ModelValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Couple_Employees.ViewModels.Index
{
    public class ProblemViewModel
    {       
        [IsEmptyFile]
        [Display(Name = "Attach text file")]
        public IFormFile TextFile { get; set; }

        [Display(Name = "Date Format")]
        public string Format { get; set; }
    }
}
