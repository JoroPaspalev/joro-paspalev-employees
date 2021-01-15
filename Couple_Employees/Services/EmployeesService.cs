using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Couple_Employees.ViewModels.Employees;
using Couple_Employees.ViewModels.Index;
using static Couple_Employees.Common.GlobalConstants;

namespace Couple_Employees.Services
{
    public class EmployeesService : IEmployeesService
    {
        public async Task<IEnumerable<CoupleEmployeesViewModel>> GetTwoEmployeesWorkedTogether(ProblemViewModel input)
        {
            var finalists = new List<CoupleEmployeesViewModel>();

            if (input.TextFile != null)
            {
                var inputData = await this.ReadAsStringAsync(input.TextFile);

                var splittedData = SplitInputData(inputData);

                List<Employee> employees;

                HashSet<int> projectIds;

                ParseAndCreateListWithEmployees(splittedData, out employees, out projectIds);

                GetEmployeesWithCalculatedDays(finalists, employees, projectIds);
            }

            return finalists;
        }

        private void GetEmployeesWithCalculatedDays(List<CoupleEmployeesViewModel> finalists, List<Employee> employees, HashSet<int> projectIds)
        {
            // Loop through Hashset and take currProjectId
            foreach (var projectId in projectIds)
            {
                int countOfEmployeesInThisProject = employees
                    .Where(x => x.ProjectId == projectId)
                    .Count();

                if (countOfEmployeesInThisProject < 2)
                {
                    continue;
                }

                // Filter Employees by Project (In Group by project)
                List<Employee> employeesByProject = employees
                     .Where(x => x.ProjectId == projectId)
                     .ToList();

                //Give me list with all couples of employees with days worked together by same project
                var employeesWithCalculatedDays = this.GetDaysWorkedTogether(employeesByProject, projectId);

                CoupleEmployeesViewModel currFinalist = employeesWithCalculatedDays
                    .OrderByDescending(x => x.WorkedDays)
                    .Take(1)
                    .First();

                if (currFinalist.WorkedDays < 1)
                {
                    continue;
                }

                finalists.Add(currFinalist);
            }
        }

        private void ParseAndCreateListWithEmployees(
            List<string> splittedData, out List<Employee> employees, out HashSet<int> projectIds)
        {
            employees = new List<Employee>();
            projectIds = new HashSet<int>();
            foreach (var row in splittedData)
            {
                List<string> currEmployee = row
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                if (currEmployee.Count < 4)
                {
                    throw new ArgumentException("InvalidData");
                }

                int empId = int.Parse(currEmployee[0]);
                int projectId = int.Parse(currEmployee[1]);

                DateTime? dateFrom = ParseDate(currEmployee[2]);
                if (dateFrom == null)
                {
                    throw new ArgumentException("InvalidData");
                }

                DateTime? dateTo;
                if (currEmployee[3] == NULL_CONST)
                {
                    dateTo = DateTime.UtcNow;
                }
                else
                {
                    dateTo = ParseDate(currEmployee[3]);
                    if (dateTo == null)
                    {
                        throw new ArgumentException("InvalidData");
                    }
                }

                var newEmployee = new Employee()
                {
                    EmpId = empId,
                    ProjectId = projectId,
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                };

                employees.Add(newEmployee);
                projectIds.Add(projectId);
            }
        }
        private List<string> SplitInputData(string inputData)
        {
            return inputData
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        private async Task<string> ReadAsStringAsync(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }

        private DateTime? ParseDate(string date)
        {
            string[] allowedFormats = { "yyyy-MM-dd", "MM-dd-yyyy", "dd-MM-yyyy", "M-dd-yyyy", "MM-d-yyyy", "M-d-yyyy", "yyyy-MM-d", "yyyy-M-dd" };

            DateTime parsedDate;

            bool isParsedDate = DateTime.TryParseExact(date, allowedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);

            return isParsedDate == true ? parsedDate : null;
        }

        private List<CoupleEmployeesViewModel> GetDaysWorkedTogether(List<Employee> employeesByProject, int projectId)
        {
            var employeesWithCalculatedDays = new List<CoupleEmployeesViewModel>();

            for (int i = 0; i < employeesByProject.Count - 1; i++)
            {
                var currEmpl = employeesByProject[i];

                for (int p = i + 1; p < employeesByProject.Count; p++)
                {
                    var nextEmpl = employeesByProject[p];

                    double daysWorkedTogether = 0;

                    if ((currEmpl.DateFrom <= nextEmpl.DateFrom &&
                         nextEmpl.DateFrom <= currEmpl.DateTo) &&
                        (currEmpl.DateTo <= nextEmpl.DateTo))
                    {
                        daysWorkedTogether = (currEmpl.DateTo - nextEmpl.DateFrom).Value.TotalDays;
                    }
                    else if ((currEmpl.DateFrom <= nextEmpl.DateFrom &&
                              nextEmpl.DateFrom <= currEmpl.DateTo) &&
                             (nextEmpl.DateTo <= currEmpl.DateTo))
                    {
                        daysWorkedTogether = (nextEmpl.DateTo - nextEmpl.DateFrom).Value.TotalDays;
                    }
                    else if ((nextEmpl.DateFrom <= currEmpl.DateFrom &&
                              currEmpl.DateFrom <= nextEmpl.DateTo) &&
                             (currEmpl.DateTo <= nextEmpl.DateTo))
                    {
                        daysWorkedTogether = (currEmpl.DateTo - currEmpl.DateFrom).Value.TotalDays;
                    }
                    else if ((nextEmpl.DateFrom <= currEmpl.DateFrom &&
                              currEmpl.DateFrom <= nextEmpl.DateTo) &&
                             (currEmpl.DateTo > nextEmpl.DateTo))
                    {
                        daysWorkedTogether = (nextEmpl.DateTo - currEmpl.DateFrom).Value.TotalDays;
                    }
                    else if ((nextEmpl.DateFrom > currEmpl.DateFrom &&
                              nextEmpl.DateFrom > currEmpl.DateTo) &&
                             (nextEmpl.DateTo > currEmpl.DateFrom &&
                              nextEmpl.DateTo > currEmpl.DateTo))
                    {
                        daysWorkedTogether = 0;
                    }
                    else if ((currEmpl.DateFrom > nextEmpl.DateFrom &&
                              currEmpl.DateFrom > nextEmpl.DateTo) &&
                             (currEmpl.DateTo > nextEmpl.DateFrom &&
                              currEmpl.DateTo > nextEmpl.DateTo))
                    {
                        daysWorkedTogether = 0;
                    }

                    var employeeWithCalculatedDays = new CoupleEmployeesViewModel
                    {
                        FirstEmployeeId = currEmpl.EmpId,
                        SecondEmployeeId = nextEmpl.EmpId,
                        ProjectId = projectId,
                        WorkedDays = (int)daysWorkedTogether
                    };

                    employeesWithCalculatedDays.Add(employeeWithCalculatedDays);
                }
            }

            return employeesWithCalculatedDays;
        }
    }
}
