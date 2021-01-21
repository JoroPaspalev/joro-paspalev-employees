using System;
using System.Globalization;

namespace CoupleEmployees.Library.ViewModels.Employees
{
    public class Employee
    {
        public int EmpId { get; set; }

        public int ProjectId { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public override string ToString()
        {
            return $"{this.EmpId} | {this.ProjectId} | {this.DateFrom?.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)} | {this.DateTo?.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}";
        }
    }
}
