using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace AgeAndGenderPrinterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Your name is: ");
            string name = Console.ReadLine();
            Console.WriteLine("Your CNP is: ");
            string cnp = Console.ReadLine();

            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Printer.PrinterClient(channel);
            var reply = await client.PrintAgeAndGenderAsync(
                          new PrintRequest { Name = name, Cnp = cnp });
            if (reply.MessageAge == "")
            {
                Console.WriteLine("You provided a faulty input.");
            }
            else
            {
                Console.WriteLine(reply.MessageName + ", you are " + reply.MessageGender + " and are " + reply.MessageAge + " years old.");
            }

            await Task.Delay(1000);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
