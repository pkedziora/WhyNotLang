using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WhyNotLang.Interpreter;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Web.Components
{
    public class EditorBase : ComponentBase
    {
        [Inject] IExecutor Executor { get; set; }
        protected TextIO textIO { get; set; }
        protected bool isRunning { get; set; } = false;
        public string programCode { get; set; } =
    @"var x:= 0
Writeln(""What is your name? "")
var name:= Readln()
while (x < 50)
begin
Writeln(ToString(x) + "", Hello "" + name)
x := x+1
Delay(100)
end
";

        protected void Stop()
        {
            Executor.Stop();
            isRunning = false;
        }

        protected async Task Execute()
        {
            textIO.Clear();
            Executor.ProgramState.Clear();
            isRunning = true;
            try
            {
                Executor.Initialise(programCode);

                await Executor.ExecuteAll();
            }
            catch (WhyNotLangException ex)
            {
                if (ex.LineNumber > 0)
                {
                     textIO.WriteLine($"[ERROR] Line {ex.LineNumber}: {ex.Message}");
                }
                else
                {
                    textIO.WriteLine($"[ERROR] {ex.Message}");
                }
            }

            isRunning = false;
        }
    }
}
