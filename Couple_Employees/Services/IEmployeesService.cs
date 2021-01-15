using System.Threading.Tasks;
using System.Collections.Generic;
using Couple_Employees.ViewModels.Index;
using Couple_Employees.ViewModels.Employees;

namespace Couple_Employees.Services
{
    public interface IEmployeesService
    {
        Task<IEnumerable<CoupleEmployeesViewModel>> GetTwoEmployeesWorkedTogether(ProblemViewModel input);
    }
}
