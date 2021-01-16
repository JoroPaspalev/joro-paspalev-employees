using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CoupleEmployees.GlobalConstants;

namespace CoupleEmployees
{
    public class CoupleEmployees
    {
        public async Task<List<CoupleEmployeesViewModel>> CalculateDaysWorkedTogether(string fileName, string dateFormat, List<string> allowedFormats)
        {            
            string inputData = await ReadDataFromFile(fileName);

            CheckDateFormat(dateFormat, allowedFormats);

            var finalists = new List<CoupleEmployeesViewModel>();

            try
            {
                finalists = (List<CoupleEmployeesViewModel>)
                this.GetTwoEmployeesWorkedTogether(inputData, dateFormat, fileName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return finalists
                .OrderByDescending(x => x.WorkedDays)
                .ToList();
        }

        public void CheckDateFormat(string dateFormat, List<string> allowedFormats)
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
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return inputData;
        }


        public IEnumerable<CoupleEmployeesViewModel>
            GetTwoEmployeesWorkedTogether(string inputData, string dateFormat, string fileName)
        {
            var finalists = new List<CoupleEmployeesViewModel>();

            CheckInputFileExtension(fileName);

            var splittedData = SplitInputData(inputData);

            List<Employee> employees;

            HashSet<int> projectIds;

            ParseAndCreateListWithEmployees(splittedData, out employees, out projectIds, dateFormat);

            GetEmployeesWithCalculatedDays(finalists, employees, projectIds);

            return finalists;
        }

        public void CheckInputFileExtension(string fileName)
        {
            if (!fileName.EndsWith(".txt"))
            {
                throw new ArgumentException(ONLY_TXT_FILES);
            }
        }

        public List<string> SplitInputData(string inputData)
        {
            return inputData
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        public void ParseAndCreateListWithEmployees(
            List<string> splittedData, out List<Employee> employees, out HashSet<int> projectIds, string dateFormat)
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

        public DateTime? ParseDate(string date, string allowedFormats)
        {
            //string[] allowedFormats = { "yyyy-MM-dd", "MM-dd-yyyy", "dd-MM-yyyy", "M-dd-yyyy", "MM-d-yyyy", "M-d-yyyy", "yyyy-MM-d", "yyyy-M-dd" };

            DateTime parsedDate;

            bool isParsedDate = DateTime.TryParseExact(date, allowedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);

            if (isParsedDate == true)
            {
                return parsedDate;
            }
            else
            {
                return null;
            }
        }

        private void CheckEndDate(DateTime? dateFrom, DateTime? dateTo)
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

        public void GetEmployeesWithCalculatedDays(List<CoupleEmployeesViewModel> finalists, List<Employee> employees, HashSet<int> projectIds)
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

        public void PrintCoupleEmployees(List<CoupleEmployeesViewModel> finalists)
        {
            var label = new string[] { "Employee ID #1", "Employee ID #2", "Project ID", "Days worked" };

            Console.WriteLine(new string('=', 60));
            Console.Write(string.Join(" | ", label));
            Console.WriteLine(" |");
            Console.WriteLine(new string('=', 60));

            var firstElementLength = label[0].Length;//14
            var secondElementLength = label[1].Length;
            var thirdElementLength = label[2].Length;
            var forthElementLength = label[3].Length;

            foreach (var currCouple in finalists)
            {
                var firstEmpLength = currCouple.FirstEmployeeId.ToString().Length;//1
                var secondEmpLength = currCouple.SecondEmployeeId.ToString().Length;
                var projectLength = currCouple.ProjectId.ToString().Length;
                var daysLength = currCouple.WorkedDays.ToString().Length;

                var firstOffset = (firstElementLength - firstEmpLength) / 2;//(14-3)2=5
                var secondOffset = (secondElementLength - secondEmpLength) / 2;
                var thirdOffset = (thirdElementLength - projectLength) / 2;
                var forthOffset = (forthElementLength - daysLength) / 2;

                var firstOffsetAfter = firstOffset;
                var secondOffsetAfter = secondOffset;
                var thirdOffsetAfter = thirdOffset;
                var forthOffsetAfter = forthOffset;

                if ((firstElementLength - firstEmpLength) % 2 != 0)//14%3 Yes
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


                var result = $"{firstOffStr}{currCouple.FirstEmployeeId}{firstOffsetAfterStr} | " +
                    $"{secondOffStr}{currCouple.SecondEmployeeId}{secondOffsetAfterStr} | " +
                    $"{thirdOffStr}{currCouple.ProjectId}{thirdOffsetAfterStr} | " +
                    $"{fourthOffStr}{currCouple.WorkedDays}{forthOffsetAfterStr} |";

                Console.WriteLine(result);
                Console.WriteLine(new string('-', 60));
            }

            Console.WriteLine(new string('=', 60));
        }
    }
}
