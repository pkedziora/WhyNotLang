using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;
using WhyNotLang.Interpreter;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Web.Components
{
    public class TextIOBase : ComponentBase
    {
        [Inject] IExecutor Executor { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }

        protected string Output { get; set; } = "";
        protected string InputBuffer { get; set; } = "";
        protected string InputValue { get; set; } = "";
        protected bool InputDisabled { get; set; } = true;

        protected override void OnAfterRender()
        {
            Executor.ProgramState.BuiltinFunctionCollection.Add("Writeln",
                async arguments =>
                {
                    var str = arguments.Single();
                    if (str.Type != ExpressionValueTypes.String)
                    {
                        throw new WhyNotLangException("String expected");
                    }

                    Output += str.Value + Environment.NewLine;
                    this.StateHasChanged();
                    await JsRuntime.InvokeAsync<string>("WhyNotLang.Text.scrollOutput");
                    return ExpressionValue.Empty;
                });

            Executor.ProgramState.BuiltinFunctionCollection.Add("Write",
                async arguments =>
                {
                    var str = arguments.Single();
                    if (str.Type != ExpressionValueTypes.String)
                    {
                        throw new WhyNotLangException("String expected");
                    }

                    Output += str.Value;
                    this.StateHasChanged();
                    await JsRuntime.InvokeAsync<string>("WhyNotLang.Text.scrollOutput");
                    return ExpressionValue.Empty;
                });

            Executor.ProgramState.BuiltinFunctionCollection.Add("Readln",
               async arguments =>
               {
                   await JsRuntime.InvokeAsync<bool>("WhyNotLang.Text.setFocus", "input");
                   InputDisabled = false;
                   this.StateHasChanged();

                   while (!Executor.Stopped && InputValue == string.Empty)
                   {
                       await Task.Delay(100);
                   }

                   var value = new ExpressionValue(InputValue, ExpressionValueTypes.String);
                   InputValue = "";
                   InputBuffer = "";
                   InputDisabled = true;

                   this.StateHasChanged();

                   return value;
               });
        }

        public void WriteLine(string line)
        {
            Output += line + "\n";
            this.StateHasChanged();
        }

        public void Clear()
        {
            Output = "";
            this.StateHasChanged();
        }

        public void OnInputKeyDown(UIKeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                InputValue = InputBuffer;
                Output += InputBuffer + "\n";
                InputBuffer = "";
            }
        }
    }
}
