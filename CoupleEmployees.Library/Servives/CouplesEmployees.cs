using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoupleEmployees.Library.ViewModels.Employees;
using static CoupleEmployees.Library.Common.GlobalConstants;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CoupleEmployees.Library.Servives
{
    public class CouplesEmployees : ICouplesEmployees
    {
        public async Task CalculateDaysWorkedTogether(
            string fileName, string dateFormat, IEnumerable<string> allowedFormats)
        {
            var inputData = await ReadDataFromFile(fileName);

            CheckDateFormat(dateFormat, allowedFormats);

            var finalists = new List<CoupleEmployeesViewModel>();
            var employees = new List<Employee>();

            try
            {
                this.GetTwoEmployeesWorkedTogether(inputData, dateFormat, fileName, finalists, employees);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            var printModel = this.GiveMePrintModelByProjects(finalists);
            this.PrintCoupleEmployees(printModel);

            var printModelEmp = this.GiveMePrintModelOfEmployees(employees);
            PrintCoupleEmployees(printModelEmp);
        }

        public IEnumerable<PrintModel> GiveMePrintModelOfEmployees(IEnumerable<Employee> employees)
        {
            var printViewModel = employees
               .Select(x => new PrintModel
               {
                   Column1Value = x.EmpId.ToString(),
                   Column2Value = x.ProjectId.ToString(),
                   Column3Value = x.DateFrom?.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                   Column4Value = x.DateTo?.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
               })
               .ToList();

            return printViewModel;
        }

        public IEnumerable<PrintModel> GiveMePrintModelByProjects(List<CoupleEmployeesViewModel> finalists)
        {
            // Rearrange Ids of employees 143|218 is == 218|143
            for (int i = 0; i < finalists.Count - 1; i++)
            {
                var ID1 = finalists[i].FirstEmployeeId;
                var ID2 = finalists[i].SecondEmployeeId;

                for (int p = i + 1; p < finalists.Count; p++)
                {
                    var nextID1 = finalists[p].FirstEmployeeId;
                    var nextID2 = finalists[p].SecondEmployeeId;

                    if (ID1 == nextID2 && ID2 == nextID1)
                    {
                        //Change their positions
                        finalists[p].FirstEmployeeId = ID1;
                        finalists[p].SecondEmployeeId = ID2;
                    }
                }
            }

            // Group by Id1 and Id2
            var longestWorkByPairs = finalists
                    .GroupBy(x => new { x.FirstEmployeeId, x.SecondEmployeeId })
                    .ToList();

            var coupleWithAllGroups = new List<CoupleEmployeesViewModel>();

            foreach (var groupedElement in longestWorkByPairs)
            {
                var projectIds = groupedElement
                    .Select(x => x.ProjectId)
                    .ToList();

                var WorkedDays = groupedElement
                    .Sum(x => x.WorkedDays);

                var coupleWithGroups = new CoupleEmployeesViewModel()
                {
                    FirstEmployeeId = groupedElement.Key.FirstEmployeeId,
                    SecondEmployeeId = groupedElement.Key.SecondEmployeeId,
                    ProjectId = string.Join(" ", projectIds),
                    WorkedDays = WorkedDays,
                };

                coupleWithAllGroups.Add(coupleWithGroups);
            }

            var printModel = coupleWithAllGroups
                .OrderByDescending(x => x.WorkedDays)
                .Select(x => new PrintModel
                {
                    Column1Value = x.FirstEmployeeId.ToString(),
                    Column2Value = x.SecondEmployeeId.ToString(),
                    Column3Value = x.ProjectId.ToString(),
                    Column4Value = x.WorkedDays.ToString(),
                });

            return printModel;
        }

        public void CheckDateFormat(string dateFormat, IEnumerable<string> allowedFormats)
        {
            if (!allowedFormats.Contains(dateFormat))
            {
                throw new ArgumentException(INVALID_DATE_FORMAT);
            }
        }

        public async Task<string> ReadDataFromFile(string fileName)
        {
            string inputData = string.Empty;

            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(fileName))
                {
                    inputData = await sr.ReadToEndAsync();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }

            return inputData;
        }

        public void GetTwoEmployeesWorkedTogether(string inputData, string dateFormat, string fileName, ICollection<CoupleEmployeesViewModel> finalists, List<Employee> employees)
        {
            CheckInputFileExtension(fileName);

            var splittedData = SplitInputData(inputData);

            HashSet<int> projectIds;

            ParseAndCreateListWithEmployees(splittedData, employees, out projectIds, dateFormat);

            GetEmployeesWithCalculatedDays(finalists, employees, projectIds);
        }

        public void CheckInputFileExtension(string fileName)
        {
            if (!fileName.EndsWith(".txt"))
            {
                throw new ArgumentException(ONLY_TXT_FILES);
            }
        }

        public IEnumerable<string> SplitInputData(string inputData)
        {
            return inputData
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        public void ParseAndCreateListWithEmployees(
            IEnumerable<string> splittedData, List<Employee> employees,
                  out HashSet<int> projectIds, string dateFormat)
        {
            projectIds = new HashSet<int>();

            foreach (var row in splittedData)
            {
                var currEmployee = row
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                if (currEmployee.Count < 4)
                {
                    throw new ArgumentException(INVALID_DATA);
                }

                int empId = int.Parse(currEmployee[0]);
                int projectId = int.Parse(currEmployee[1]);

                DateTime? dateFrom = ParseDate(currEmployee[2], dateFormat);
                if (dateFrom == null)
                {
                    throw new ArgumentException(INVALID_START_DATE);
                }

                DateTime? dateTo;
                if (currEmployee[3] == NULL_CONST)
                {
                    dateTo = DateTime.UtcNow;
                }
                else
                {
                    dateTo = ParseDate(currEmployee[3], dateFormat);
                    if (dateTo == null)
                    {
                        throw new ArgumentException(INVALID_END_DATE);
                    }
                }

                CheckEndDate(dateFrom, dateTo);

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

        public DateTime? ParseDate(string date, string dateFormat)
        {
            DateTime parsedDate;

            bool isParsedDate = DateTime.TryParseExact(date, dateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out parsedDate);

            if (isParsedDate)
            {
                return parsedDate;
            }
            else
            {
                return null;
            }
        }

        public void CheckEndDate(DateTime? dateFrom, DateTime? dateTo)
        {
            if ((dateTo <= dateFrom))
            {
                throw new ArgumentException(END_DATE_AFTER_START_DATE);
            }

            if ((dateTo > DateTime.UtcNow))
            {
                throw new ArgumentException(END_DATE_IS_IN_FUTURE);
            }
        }

        public void GetEmployeesWithCalculatedDays(ICollection<CoupleEmployeesViewModel> finalists, IEnumerable<Employee> employees, ICollection<int> projectIds)
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
                var employeesByProject = employees
                     .Where(x => x.ProjectId == projectId)
                     .ToList();

                //Give me list with all couples of employees with days worked together by same project
                var employeesWithCalculatedDays = this.GetDaysWorkedTogether(employeesByProject, projectId);

                foreach (var couple in employeesWithCalculatedDays)
                {
                    if (couple.WorkedDays < 1)
                    {
                        continue;
                    }

                    finalists.Add(couple);
                }
            }

            finalists = finalists
                .OrderByDescending(x => x.WorkedDays)
                .ToList();
        }

        public List<CoupleEmployeesViewModel> GetDaysWorkedTogether(List<Employee> employeesByProject, int projectId)
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

                    var employeeWithCalculatedDays = new CoupleEmployeesViewModel
                    {
                        FirstEmployeeId = currEmpl.EmpId,
                        SecondEmployeeId = nextEmpl.EmpId,
                        ProjectId = projectId.ToString(),
                        WorkedDays = (int)daysWorkedTogether,
                    };

                    employeesWithCalculatedDays.Add(employeeWithCalculatedDays);
                }
            }

            return employeesWithCalculatedDays;
        }

        public void PrintCoupleEmployees(IEnumerable<PrintModel> finalists)
        {
            var labelWithProjects = new string[] { "  Employee ID #1  ", "  Employee ID #2  ", "   Project/s ID   ", "  Days worked  " };

            var labelWithDates = new string[] { "   Employee ID    ", "    Project ID    ", "     DateFrom     ", "    DateTo     " };

            var lengthOfLine = labelWithProjects.Sum(x => x.Length) + 11;

            int value;

            if (int.TryParse(finalists.First().Column4Value, out value))
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Employees worked the longest on common projects");
                PrintDoubleLine(lengthOfLine);
                Console.Write(string.Join(" | ", labelWithProjects));
            }
            else
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("All employees worked by projects");
                PrintDoubleLine(lengthOfLine);
                Console.Write(string.Join(" | ", labelWithDates));
            }

            Console.WriteLine(" |");
            PrintDoubleLine(lengthOfLine);

            var firstElementLength = labelWithProjects[0].Length;
            var secondElementLength = labelWithProjects[1].Length;
            var thirdElementLength = labelWithProjects[2].Length;
            var forthElementLength = labelWithProjects[3].Length;

            foreach (var currCouple in finalists)
            {
                var firstEmpLength = currCouple.Column1Value.Length;
                var secondEmpLength = currCouple.Column2Value.Length;
                var projectLength = currCouple.Column3Value.Length;
                var daysLength = currCouple.Column4Value.Length;

                var firstOffset = (firstElementLength - firstEmpLength) / 2;
                var secondOffset = (secondElementLength - secondEmpLength) / 2;
                var thirdOffset = (thirdElementLength - projectLength) / 2;
                var forthOffset = (forthElementLength - daysLength) / 2;

                var firstOffsetAfter = firstOffset;
                var secondOffsetAfter = secondOffset;
                var thirdOffsetAfter = thirdOffset;
                var forthOffsetAfter = forthOffset;

                if ((firstElementLength - firstEmpLength) % 2 != 0)
                {
                    firstOffsetAfter++;
                }

                if ((secondElementLength - secondEmpLength) % 2 != 0)
                {
                    secondOffsetAfter++;
                }

                if (((thirdElementLength - projectLength) % 2 != 0))
                {
                    thirdOffsetAfter++;
                }

                if (((forthElementLength - daysLength) % 2 != 0))
                {
                    forthOffsetAfter++;
                }

                var firstOffStr = new string(' ', firstOffset);
                var secondOffStr = new string(' ', secondOffset);
                var thirdOffStr = new string(' ', thirdOffset);
                var fourthOffStr = new string(' ', forthOffset);

                var firstOffsetAfterStr = new string(' ', firstOffsetAfter);
                var secondOffsetAfterStr = new string(' ', secondOffsetAfter);
                var thirdOffsetAfterStr = new string(' ', thirdOffsetAfter);
                var forthOffsetAfterStr = new string(' ', forthOffsetAfter);


                var result = $"{firstOffStr}{currCouple.Column1Value}{firstOffsetAfterStr} | " +
                    $"{secondOffStr}{currCouple.Column2Value}{secondOffsetAfterStr} | " +
                    $"{thirdOffStr}{currCouple.Column3Value}{thirdOffsetAfterStr} | " +
                    $"{fourthOffStr}{currCouple.Column4Value}{forthOffsetAfterStr} |";

                Console.WriteLine(result);
                Console.WriteLine(new string('-', lengthOfLine));
            }

            PrintDoubleLine(lengthOfLine);
        }

        public async Task<string> ReadAsStringAsync(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }

        public void PrintDoubleLine(int length)
        {
            Console.WriteLine(new string('=', length));
        }       
    }
}
