using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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
                    Console.Write("Please enter path to text file:");
                    string fileName = Console.ReadLine();

                    var allowedFormats = new List<string>()
                    {
                      "yyyy-MM-dd", "yyyy-MM-d",
                      "yyyy-M-dd", "M-dd-yyyy",
                      "MM-d-yyyy", "M-d-yyyy",
                      "MM-dd-yyyy", "dd-MM-yyyy",
                      "dd-M-yyyy", "d-MM-yyyy",
                      "d-M-yyyy", "dddd,dd-MMMM-yyyy",
                      "yyyy MMMM", "yyyy-MMMM",
                      "dddd,dd-MM-yyyy"
                    };

                    Console.WriteLine("Valid Date formats: ");
                    Console.WriteLine(string.Join(Environment.NewLine, allowedFormats));
                    Console.Write("Please select Date format:");
                    string dateFormat = Console.ReadLine();

                    var coupleEmployees = new CoupleEmployees();

                    var finalists = await coupleEmployees
                        .CalculateDaysWorkedTogether(fileName, dateFormat, allowedFormats);

                    coupleEmployees.PrintCoupleEmployees(finalists);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
