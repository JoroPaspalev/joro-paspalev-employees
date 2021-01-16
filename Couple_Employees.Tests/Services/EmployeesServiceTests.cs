using System;
using System.Collections.Generic;
using Xunit;
using Couple_Employees.Services;
using Couple_Employees.ViewModels.Employees;

namespace Couple_Employees.Tests.Services
{
    public class EmployeesServiceTests
    {
        [Fact]
        public void SplitInputDataShouldReturnListWithSplittedStrings()
        {
            string inputData = "143, 1, 31-12-2013, 02-04-2018\r\n218, 1, 11-12-2011, 22-03-2015\r\n144, 1, 14-04-2001, 19-07-2003\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n";

            var employeesService = new EmployeesService();

            var splittedData = employeesService.SplitInputData(inputData);

            var result = new List<string>()
            {
                "143, 1, 31-12-2013, 02-04-2018",
                "218, 1, 11-12-2011, 22-03-2015",
                "144, 1, 14-04-2001, 19-07-2003"
            };

            Assert.Equal<string>(result, splittedData);
        }

        [Fact]
        public void ParseAndCreateListWithEmployeesShouldReturnListWithEmployees()
        {
            string inputData = "143, 1, 31-12-2013, 02-04-2018\r\n218, 1, 11-12-2011, 22-03-2015\r\n144, 1, 14-04-2001, 19-07-2003\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n";

            string dateFormat = "dd-MM-yyyy";

            var employeesService = new EmployeesService();

            var splittedData = employeesService.SplitInputData(inputData);

            List<Employee> employees;

            HashSet<int> projectIds;

            employeesService.ParseAndCreateListWithEmployees
                (splittedData, out employees, out projectIds, dateFormat);

            List<Employee> employeesResult = new List<Employee>()
            {
                new Employee
                {
                    EmpId = 143,
                    ProjectId = 1,
                    DateFrom = new DateTime(2013, 12, 31),
                    DateTo = new DateTime(2018, 4, 2 )
                },
                 new Employee
                {
                    EmpId = 218,
                    ProjectId = 1,
                    DateFrom = new DateTime(2011, 12, 11),
                    DateTo = new DateTime(2015, 3, 22 )
                },
                  new Employee
                {
                    EmpId = 144,
                    ProjectId = 1,
                    DateFrom = new DateTime(2001, 4, 14),
                    DateTo = new DateTime(2003, 7, 19 )
                }
            };

            HashSet<int> projectIdsResult = new HashSet<int>() { 1 };

            Assert.Equal<int>(projectIdsResult, projectIds);
            Assert.Equal(employeesResult[0].EmpId, employees[0].EmpId);
            Assert.Equal(employeesResult[0].ProjectId, employees[0].ProjectId);
            Assert.Equal(employeesResult[0].DateFrom, employees[0].DateFrom);
            Assert.Equal(employeesResult[0].DateTo, employees[0].DateTo);

            Assert.Equal(employeesResult[1].EmpId, employees[1].EmpId);
            Assert.Equal(employeesResult[1].ProjectId, employees[1].ProjectId);
            Assert.Equal(employeesResult[1].DateFrom, employees[1].DateFrom);
            Assert.Equal(employeesResult[1].DateTo, employees[1].DateTo);

            Assert.Equal(employeesResult[2].EmpId, employees[2].EmpId);
            Assert.Equal(employeesResult[2].ProjectId, employees[2].ProjectId);
            Assert.Equal(employeesResult[2].DateFrom, employees[2].DateFrom);
            Assert.Equal(employeesResult[2].DateTo, employees[2].DateTo);
        }

        [Fact]
        public void GetEmployeesWithCalculatedDaysShouldCalculateWorkedDays()
        {
            string inputData = "143, 1, 31-12-2013, 02-04-2018\r\n218, 1, 11-12-2011, 22-03-2015\r\n144, 1, 14-04-2001, 19-07-2003\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n";

            string dateFormat = "dd-MM-yyyy";

            var employeesService = new EmployeesService();

            var splittedData = employeesService.SplitInputData(inputData);

            List<Employee> employees;

            HashSet<int> projectIds;

            employeesService.ParseAndCreateListWithEmployees
                (splittedData, out employees, out projectIds, dateFormat);

            var finalists = new List<CoupleEmployeesViewModel>();
            employeesService.GetEmployeesWithCalculatedDays(finalists, employees, projectIds);

            var finalistsResult = new List<CoupleEmployeesViewModel>()
            {
                new CoupleEmployeesViewModel
                {
                    FirstEmployeeId = 143,
                    SecondEmployeeId = 218,
                    ProjectId = 1,
                    WorkedDays = 446
                }
            };

            Assert.Equal(finalists[0].FirstEmployeeId, finalistsResult[0].FirstEmployeeId);
            Assert.Equal(finalists[0].SecondEmployeeId, finalistsResult[0].SecondEmployeeId);
            Assert.Equal(finalists[0].ProjectId, finalistsResult[0].ProjectId);
            Assert.Equal(finalists[0].WorkedDays, finalistsResult[0].WorkedDays);
        }

        [Fact]
        public void ParseDateShouldReturnDateTimeWhenIsCorrectData()
        {
            var employeesService = new EmployeesService();

            string dateAsString = "2013-11-01";

            string allowedFormats = "yyyy-MM-dd";

            DateTime? parsedDate = employeesService.ParseDate(dateAsString, allowedFormats);

            DateTime? result = new DateTime(2013, 11, 1);

            Assert.Equal(result, parsedDate);           
        }

        [Fact]
        public void ParseDateShouldReturnNullWhenInputIsIncorrect()
        {
            var employeesService = new EmployeesService();

            string dateAsString = "text";

            string allowedFormats = "yyyy-MM-dd";

            DateTime? parsedDate = employeesService.ParseDate(dateAsString, allowedFormats);

            DateTime? result = null;

            Assert.Equal(result, parsedDate);
        }
    }
}
