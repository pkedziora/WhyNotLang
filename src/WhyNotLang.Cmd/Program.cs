using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter;

namespace WhyNotLang.Cmd
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string fileName;
            if(System.Diagnostics.Debugger.IsAttached)
            {
                fileName = "Samples/Test.wnl";
            }
            else
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Plese provide path to the program to execute");
                    return;
                }
                
                fileName = args[0];
            }
            
            var serviceProvider = IoC.BuildServiceProvider();
            serviceProvider.AddConsoleInputOutput();
            
            var executor = serviceProvider.GetService<IExecutor>();
            var program = File.ReadAllText(fileName);
            executor.Initialise(program);
            await executor.ExecuteAll();

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }
    }
}