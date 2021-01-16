using Microsoft.AspNetCore.Mvc;

namespace Couple_Employees.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult InvalidData(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
