using System.Threading.Tasks;
using System.Collections.Generic;
using CoupleEmployees.Library.ViewModels.Index;
using CoupleEmployees.Library.ViewModels.Employees;

namespace Couple_Employees.Services
{
    public interface IEmployeesService
    {
        Task<IEnumerable<CoupleEmployeesViewModel>> GetTwoEmployeesWorkedTogether(ProblemViewModel input);
    }
}
