using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Couple_Employees.Services;
using Couple_Employees.ViewModels.Employees;
using Couple_Employees.ViewModels.Index;
using Couple_Employees.ViewModels.Error;
using static Couple_Employees.Common.GlobalConstants;

namespace Couple_Employees.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }

        [HttpPost]
        public async Task<IActionResult> CoupleEmployees(ProblemViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction
                    ("InvalidData", "Errors", new ErrorMessage { Message = EMPTY_FILE });
            }

            var finalists = new List<CoupleEmployeesViewModel>();

            try
            {
                finalists = (List<CoupleEmployeesViewModel>)
                await this.employeesService.GetTwoEmployeesWorkedTogether(input);
            }
            catch (Exception ex)
            {
                return this.RedirectToAction("InvalidData", "Errors", new ErrorMessage { Message = ex.Message });
            }

            return this.View(finalists.OrderByDescending(x => x.WorkedDays));
        }
    }
}
