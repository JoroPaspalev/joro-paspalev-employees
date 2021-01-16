using System;

namespace CoupleEmployees
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
