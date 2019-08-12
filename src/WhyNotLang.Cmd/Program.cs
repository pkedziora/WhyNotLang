using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using WhyNotLang.EmbeddedResources.Reader;
using WhyNotLang.Interpreter;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Cmd
{
    class Program
    {
        static IExecutor _executor;
        static IResourceReader _sampleReader;

        static Program()
        {
            Initialise();
        }

        static async Task Main(string[] args)
        {
            string programName;
            string program;
            if (System.Diagnostics.Debugger.IsAttached)
            {
                programName = "QuickSort";
                program = _sampleReader.ReadSample(programName);
            }
            else
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Please provide path to the program to execute");
                    return;
                }

                programName = args[0];
                program = File.ReadAllText(programName);
            }

            try
            {
                _executor.Initialise(program);
                await _executor.ExecuteAll();
            }
            catch (WhyNotLangException ex)
            {
                Console.WriteLine(ex.LineNumber > 0
                    ? $"[ERROR] Line {ex.LineNumber}: {ex.Message}"
                    : $"[ERROR] {ex.Message}");
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
            _executor = serviceProvider.GetService<IExecutor>();
            _sampleReader = serviceProvider.GetService<IResourceReader>();
        }
    }
}