using Couple_Employees.Services;
using Couple_Employees.ViewModels.Employees;
using Couple_Employees.ViewModels.Index;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Couple_Employees.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService employeeService;

        public EmployeesController(IEmployeesService employeeService)
        {
            this.employeeService = employeeService;
        }



        [HttpPost]
        public async Task<IActionResult> CoupleEmployees(ProblemViewModel input)
        {
            // Трябва да валидирам дали разширението е .txt
            //if (!ModelState.IsValid)
            //{
            //    var shipmentForEdit = this.driversService.GetShipment(input.ShipmentId);

            //    return this.View(shipmentForEdit);
            //}

            var finalists = new List<CoupleEmployeesViewModel>();

            if (input.TextFile != null)
            {
                var index = input.TextFile.FileName.LastIndexOf('.');
                var extension = input.TextFile.FileName.Substring(index);



                // Дай ми стринг с данните
                string inputData = await this.employeeService.ReadAsStringAsync(input.TextFile);
                var splittedData = inputData.
                    Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                var employees = new List<Employee>();
                var projectIds = new HashSet<int>();

                foreach (var row in splittedData)
                {
                    List<string> currEmployee = row
                        .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    int empId = int.Parse(currEmployee[0]);
                    int projectId = int.Parse(currEmployee[1]);
                    DateTime? dateFrom = ParseDate(currEmployee[2]);
                    DateTime? dateTo = DateTime.UtcNow;

                    //TODO make constant for NULL
                    if (currEmployee[3] != "NULL")
                    {
                        dateTo = ParseDate(currEmployee[3]);
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

                ;



                foreach (var projectId in projectIds)
                {
                    int countOfEmployeesInThisProject = employees
                        .Where(x => x.ProjectId == projectId)
                        .Count();

                    if (countOfEmployeesInThisProject < 2)
                    {
                        continue;
                    }

                    List<Employee> firstTwoEmployees = employees
                         .Where(x => x.ProjectId == projectId)
                         .OrderByDescending(x => x.WorkingDays)
                         .Take(2)
                         .ToList();

                    var newFinalist = new CoupleEmployeesViewModel
                    {
                        FirstEmployeeId = firstTwoEmployees[0].EmpId,
                        SecondEmployeeId = firstTwoEmployees[1].EmpId,
                        ProjectId = firstTwoEmployees[0].ProjectId,
                        WorkedDays = firstTwoEmployees[0].WorkingDays + firstTwoEmployees[1].WorkingDays
                    };

                    finalists.Add(newFinalist);
                }

                //var dddd = employees
                //     .OrderBy(x => x.ProjectId)
                //     .ThenByDescending(x => x.WorkingDays)
                //     .GroupBy(x =>x.ProjectId)
                //     .
                //     .ToList();

                ;




                //var sb = new StringBuilder();

                //using (var reader = new StreamReader(input.TextFile.OpenReadStream()))
                //{
                //    while (reader.Peek() >= 0)
                //        sb.AppendLine(reader.ReadLine());
                //}

                //var ss = sb.ToString().TrimEnd();

                //;





                //Encoding.UTF8.GetBytes(input.TextFile);


                ////Отвори ми файлов стрийм към wwwroot/proof/име на файла в режим на създаване на нов файл и вземи данните от Picture и ми ги копирай в посочения stream
                //string imageUrl = "/proof/" + currImage.Id + "." + extension;

                //using (FileStream fileStream = new FileStream(this.webHostEnvironment.WebRootPath + imageUrl, FileMode.Create))
                //{
                //    await picture.CopyToAsync(fileStream);
                //}

                //currImage.ImageUrl = imageUrl;

                //input.Images.Add(currImage);

            }

            return this.View(finalists);


        }

        public DateTime? ParseDate(string date)
        {
            string[] allowedFormats = { "yyyy-MM-dd", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy" };

            DateTime parsedDateTo;

            bool isParsedDate = DateTime.TryParseExact(date, allowedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTo);

            return isParsedDate == true ? parsedDateTo : null;
        }
    }
}
