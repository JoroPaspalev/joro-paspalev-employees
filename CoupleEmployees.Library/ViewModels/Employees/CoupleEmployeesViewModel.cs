namespace CoupleEmployees.Library.ViewModels.Employees
{
    public class CoupleEmployeesViewModel : CoupleEmployeesBase
    {      
        public override string ToString()
        {
            return $"{this.FirstEmployeeId} | {this.SecondEmployeeId} | {this.ProjectId} | {this.WorkedDays}";
        }
    }
}
