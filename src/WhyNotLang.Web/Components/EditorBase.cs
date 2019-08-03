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
    @"function OnKeyUp(key)
begin
if (key == ""d"")
xd := 0
else if (key == ""a"")
xd := 0
else if (key == ""w"")
yd := 0
else if (key == ""s"")
yd:= 0
end

function OnKeyDown(key)
begin
if (key == ""d"")
xd := 5
else if (key == ""a"")
xd := -5
else if (key == ""w"")
yd := -5
else if (key == ""s"")
yd:= 5
end

global x := 0
global y := 0
global xd := 0
global yd := 0

InitGraphics()
while(1)
begin
ClearScreen(""black"")
x := x + xd
y := y + yd
DrawRectangle(x, y,50,70,""red"")
DrawText(""Hello World"", x, y + 100, ""cyan"", ""normal 30px Calibri"")
Delay(1)
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
                string msg;
                if (ex.LineNumber > 0)
                {
                    msg = $"[ERROR] Line {ex.LineNumber}: {ex.Message}";
                }
                else
                {
                    msg = $"[ERROR] {ex.Message}";
                }

                textIO.WriteLine(msg);
                Console.WriteLine(msg);
            }

            isRunning = false;
        }
    }
}
