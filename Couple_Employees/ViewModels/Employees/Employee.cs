using System;

namespace Couple_Employees.ViewModels.Employees
{
    public class Employee
    {
        public int EmpId { get; set; }

        public int ProjectId { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public double WorkingDays { get; set; }
    }
}
