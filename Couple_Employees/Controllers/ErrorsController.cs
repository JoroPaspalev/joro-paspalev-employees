using Microsoft.AspNetCore.Mvc;
using static Couple_Employees.Common.GlobalConstants;

namespace Couple_Employees.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult InvalidData()
        {
            ViewBag.ErrorMessage = INVALID_DATA;
            return View();
        }

        public IActionResult InvalidTextFile()
        {
            ViewBag.ErrorMessage = ONLY_TXT_FILES;
            return View();
        }
    }
}
