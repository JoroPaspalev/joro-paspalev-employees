using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoupleEmployees.Library.Servives;

namespace CoupleEmployees
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Write("Please enter the path to text file:");
                    string fileName = Console.ReadLine();

                    var allowedFormats = new List<string>()
                    {
                      "yyyy-MM-dd", "yyyy-MM-d",
                      "yyyy-M-dd", "M-dd-yyyy",
                      "MM-d-yyyy", "M-d-yyyy",
                      "MM-dd-yyyy", "dd-MM-yyyy",
                      "MMMM-dd-yyyy", "yyyy-dd-MM",
                      "dd-M-yyyy", "d-MM-yyyy",
                      "d-M-yyyy", "dddd,dd-MMMM-yyyy",
                      "yyyy MMMM", "yyyy-MMMM",
                      "dddd,dd-MM-yyyy", "yyyy/MM/dd",
                      "yyyy/dd/MM", "MM/dd/yyyy",
                      "MMM/dd/yyyy", "MMMM/dd/yyyy",
                      "yyyy.MM.dd", "yyyy.dd.MM",
                      "MM.dd.yyyy","MMM.dd.yyyy",
                      "MM.dd.yyyy", "MMMM.dd.yyyy"
                    };

                    Console.WriteLine("Valid Date formats: ");
                    Console.WriteLine(string.Join(Environment.NewLine, allowedFormats));
                    Console.Write("Please select Date format:");
                    string dateFormat = Console.ReadLine();

                    var coupleEmployees = new CouplesEmployees();

                    await coupleEmployees
                        .CalculateDaysWorkedTogether(fileName, dateFormat, allowedFormats);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
