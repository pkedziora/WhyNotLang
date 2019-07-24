using System;
using System.IO;
using WhyNotLang.Interpreter.State;

namespace WhyNotLang.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var programState = new ProgramState();
            var executor = ExecutorFactory.CreateExecutor(programState);

            var program = File.ReadAllText("Samples/Test.wnl");
            executor.Initialise(program);
            executor.ExecuteAll();
            Console.ReadLine();
        }
    }
}