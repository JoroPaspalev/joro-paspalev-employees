using Couple_Employees.Services;
using Couple_Employees.ViewModels.Employees;
using Couple_Employees.ViewModels.Index;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static Couple_Employees.Common.GlobalConstants;

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

            List<CoupleEmployeesViewModel> finalists = new List<CoupleEmployeesViewModel>();

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

                    if (currEmployee[3] != NULL_CONST)
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

                // Loop thrugh Hashset and take currProjectId
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
                                      nextEmpl.DateFrom >currEmpl.DateTo) && 
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
                                WorkedDays = daysWorkedTogether
                            };

                            employeesWithCalculatedDays.Add(employeeWithCalculatedDays);
                        }
                    }

                    // Направи ми един лист само с служителите участващи в текущия проект, сортирай го Descending по Working Days и вземи първия ред.

                    CoupleEmployeesViewModel currFinalist = employeesWithCalculatedDays
                        .OrderByDescending(x => x.WorkedDays)
                        .Take(1)
                        .First();

                    finalists.Add(currFinalist);

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

            return this.View(finalists.OrderByDescending(x=>x.WorkedDays));


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
