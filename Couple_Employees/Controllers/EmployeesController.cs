using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CoupleEmployees.Library.ViewModels.Employees;
using CoupleEmployees.Library.ViewModels.Index;
using CoupleEmployees.Library.ViewModels.Error;
using static CoupleEmployees.Library.Common.GlobalConstants;
using CoupleEmployees.Library.Servives;
using Couple_Employees.ViewModels;

namespace Couple_Employees.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICouplesEmployees couplesEmployees;

        public EmployeesController(ICouplesEmployees couplesEmployees)
        {
            this.couplesEmployees = couplesEmployees;
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
            var employees = new List<Employee>();
            var printModel = new PrintViewModel();

            try
            {                
                couplesEmployees.CheckInputFileExtension(input.TextFile.FileName);

                var inputData = await couplesEmployees.ReadAsStringAsync(input.TextFile);

                var splittedData = couplesEmployees.SplitInputData(inputData);

                couplesEmployees.GetTwoEmployeesWorkedTogether(
                    inputData, input.Format, input.TextFile.FileName, finalists, employees);

                var AllEmployeesByProjects = couplesEmployees.GiveMePrintModelByProjects(finalists);
              
                var allEmployees = couplesEmployees.GiveMePrintModelOfEmployees(employees);

                printModel.AllEmployees = allEmployees;
                printModel.AllEmployeesByProjects = AllEmployeesByProjects;                
            }
            catch (Exception ex)
            {
                return this.RedirectToAction("InvalidData", "Errors", new ErrorMessage { Message = ex.Message });
            }

            return this.View(printModel);
        }
    }
}
