using System;
using System.Collections.Generic;
using System.Text;

namespace CoupleEmployees.Library.ViewModels.Employees
{
    public class CoupleEmployeesBase : ICoupleEmployees
    {
        public int FirstEmployeeId { get; set; }

        public int SecondEmployeeId { get; set; }

        public string ProjectId { get; set; }

        public int WorkedDays { get; set; }
    }
}
