using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgeAndGenderPrinter.Services
{
    public class PrinterService : Printer.PrinterBase
    {
        private int GetAge(char gender, string dateOfBirth)
        {
            string completeDateOfBirth;

            if (gender == '1' || gender == '2')
            {
                completeDateOfBirth = "19" + dateOfBirth;
            }
            else
            {
                completeDateOfBirth = "20" + dateOfBirth;
            }

            return (Int32.Parse(DateTime.Today.ToString("yyyyMMdd")) -
                Int32.Parse(completeDateOfBirth)) / 10000;
        }

        private bool ValidateCNP(string cnp)
        {
            if (!"1256".Contains(cnp.ElementAt(0)))
                return false;
            if (Int32.Parse(cnp.Substring(3, 2)) > 12)
                return false;
            if (Int32.Parse(cnp.Substring(5, 2)) > 31)
                return false;
            return true;
        }

        public override Task<PrintReply> PrintAgeAndGender(PrintRequest request, ServerCallContext context)
        {
            string gender = "";
            string age = "";

            if (request.Cnp.Length >= 7 && ValidateCNP(request.Cnp))
            {
                var genderChar = request.Cnp.ElementAt(0);
                var dateOfBirth = request.Cnp.Substring(1, 6);

                if (genderChar == '1' || genderChar == '5')
                {
                    gender = "male";
                }
                else
                {
                    if (genderChar == '2' || genderChar == '6')
                    {
                        gender = "female";
                    }
                }

                age = GetAge(genderChar, dateOfBirth).ToString();
                Console.WriteLine(request.Name + " is " + gender + " and is " + age + " years old.");
            }
            else
            {
                Console.WriteLine(request.Name + " provided a faulty input.");
            }

            return Task.FromResult(new PrintReply
            {
                MessageName = request.Name,
                MessageAge = age,
                MessageGender = gender
            });
        }
    }
}

