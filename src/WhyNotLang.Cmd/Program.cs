using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter;
using WhyNotLang.Samples.Reader;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Cmd
{
    class Program
    {
        static IExecutor Executor;
        static ISampleReader SampleReader;

        static Program()
        {
            Initialise();
        }

        static async Task Main(string[] args)
        {
            string fileName;
            string program;
            if(System.Diagnostics.Debugger.IsAttached)
            {
                fileName = "Fibonacci.wnl";
                program = SampleReader.Read(fileName);
            }
            else
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Plese provide path to the program to execute");
                    return;
                }
                
                fileName = args[0];
                program = File.ReadAllText(fileName);
            }
            
            try
            {
                Executor.Initialise(program);
                await Executor.ExecuteAll();
            }
            catch (WhyNotLangException ex)
            {
                if (ex.LineNumber > 0)
                {
                    Console.WriteLine($"[ERROR] Line {ex.LineNumber}: {ex.Message}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] {ex.Message}");
                }
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }

        static void Initialise()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            serviceProvider.AddConsoleInputOutput();
            Executor = serviceProvider.GetService<IExecutor>();
            SampleReader = serviceProvider.GetService<ISampleReader>();
        }
    }
}