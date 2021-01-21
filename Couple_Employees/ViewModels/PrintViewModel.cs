using CoupleEmployees.Library.ViewModels.Employees;
using System.Collections.Generic;

namespace Couple_Employees.ViewModels
{
    public class PrintViewModel
    {
        public IEnumerable<PrintModel> AllEmployees { get; set; }

        public IEnumerable<PrintModel> AllEmployeesByProjects { get; set; }
    }
}
