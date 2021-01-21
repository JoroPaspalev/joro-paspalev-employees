using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoupleEmployees.Library.ViewModels.Employees;
using Microsoft.AspNetCore.Http;

namespace CoupleEmployees.Library.Servives
{
    public interface ICouplesEmployees
    {
        Task CalculateDaysWorkedTogether(string fileName, string dateFormat, IEnumerable<string> allowedFormats);

        IEnumerable<PrintModel> GiveMePrintModelOfEmployees(IEnumerable<Employee> employees);

        IEnumerable<PrintModel> GiveMePrintModelByProjects(List<CoupleEmployeesViewModel> finalists);

        void CheckDateFormat(string dateFormat, IEnumerable<string> allowedFormats);

        Task<string> ReadDataFromFile(string fileName);

        void GetTwoEmployeesWorkedTogether(string inputData, string dateFormat, string fileName, ICollection<CoupleEmployeesViewModel> finalists, List<Employee> employees);

        void CheckInputFileExtension(string fileName);

        IEnumerable<string> SplitInputData(string inputData);

        void ParseAndCreateListWithEmployees(IEnumerable<string> splittedData, List<Employee> employees,
                 out HashSet<int> projectIds, string dateFormat);

        DateTime? ParseDate(string date, string dateFormat);

        void CheckEndDate(DateTime? dateFrom, DateTime? dateTo);

        void GetEmployeesWithCalculatedDays(ICollection<CoupleEmployeesViewModel> finalists,
            IEnumerable<Employee> employees, ICollection<int> projectIds);

        List<CoupleEmployeesViewModel> GetDaysWorkedTogether(List<Employee> employeesByProject, int projectId);

        void PrintCoupleEmployees(IEnumerable<PrintModel> finalists);

        Task<string> ReadAsStringAsync(IFormFile file);

        void PrintDoubleLine(int length);
    }
}
