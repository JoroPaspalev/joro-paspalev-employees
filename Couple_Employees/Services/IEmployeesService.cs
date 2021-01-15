using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Couple_Employees.Services
{
    public interface IEmployeesService
    {
        Task<string> ReadAsStringAsync(IFormFile file);
    }
}
