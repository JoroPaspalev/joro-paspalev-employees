namespace CoupleEmployees
{
    public class CoupleEmployeesViewModel
    {
        public int FirstEmployeeId { get; set; }

        public int SecondEmployeeId { get; set; }

        public int ProjectId { get; set; }

        public int WorkedDays { get; set; }

        public override string ToString()
        {
            return $"{this.FirstEmployeeId} | {this.SecondEmployeeId} | {this.ProjectId} | {this.WorkedDays}";
        }
    }
}
