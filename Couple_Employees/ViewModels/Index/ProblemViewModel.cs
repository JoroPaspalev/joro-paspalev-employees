using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Couple_Employees.ViewModels.Index
{
    public class ProblemViewModel
    {
        //TODO
        //[Accept_Only_jpg_pngAttribute]
        //[ValidateImageSizeAttribute(2)]
        [Display(Name = "Прикачи текстов файл")]
        public IFormFile TextFile { get; set; }
    }
}
