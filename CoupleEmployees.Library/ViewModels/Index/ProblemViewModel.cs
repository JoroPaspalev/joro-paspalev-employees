using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using CoupleEmployees.Library.Attributes.ModelValidationAttributes;

namespace CoupleEmployees.Library.ViewModels.Index
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
