using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Web.Components
{
    public class CanvasIOBase: ComponentBase
    {
        [Inject] IExecutor Executor { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync()
        {
            var canvasId = "myCanvas";
            await JsRuntime.InvokeAsync<string>("WhyNotLang.Canvas.initGraphics", canvasId);

            Executor.ProgramState.BuiltinFunctionCollection.Add("ClearScreen",
                async arguments =>
                {
                    await JsRuntime.InvokeAsync<string>("WhyNotLang.Canvas.clearScreen");
                    return ExpressionValue.Empty;
                });

            Executor.ProgramState.BuiltinFunctionCollection.Add("DrawRectangle",
                async arguments =>
                {
                    var x = arguments[0];
                    var y = arguments[1];
                    var width = arguments[2];
                    var height = arguments[3];
                    var color = arguments[4];
                    if (x.Type != ExpressionValueTypes.Number || y.Type != ExpressionValueTypes.Number 
                    || width.Type != ExpressionValueTypes.Number || height.Type != ExpressionValueTypes.Number)
                    {
                        throw new WhyNotLangException("Number expected");
                    }

                    if (color.Type != ExpressionValueTypes.String)
                    {
                        throw new WhyNotLangException("String expected");
                    }

                    await JsRuntime.InvokeAsync<string>("WhyNotLang.Canvas.drawRectangle", 
                        x.Value, y.Value, width.Value, height.Value, color.Value);
                    return ExpressionValue.Empty;
                });

            Executor.ProgramState.BuiltinFunctionCollection.Add("DrawText",
                async arguments =>
                {
                    var text = arguments[0];
                    var x = arguments[1];
                    var y = arguments[2];
                    var color = arguments[3];
                    var font = arguments[4];
                    if (x.Type != ExpressionValueTypes.Number || y.Type != ExpressionValueTypes.Number)
                    {
                        throw new WhyNotLangException("Number expected");
                    }

                    if (text.Type != ExpressionValueTypes.String || color.Type != ExpressionValueTypes.String
                    || font.Type != ExpressionValueTypes.String)
                    {
                        throw new WhyNotLangException("String expected");
                    }

                    await JsRuntime.InvokeAsync<string>("WhyNotLang.Canvas.drawText",
                        text.Value, x.Value, y.Value, color.Value, font.Value);
                    return ExpressionValue.Empty;
                });
        }

        [JSInvokable]
        public static Task OnKeyDown(string key)
        {
            return CallKeyEventFunction(key, "OnKeyDown");
        }

        [JSInvokable]
        public static Task OnKeyUp(string key)
        {
            return CallKeyEventFunction(key, "OnKeyUp");
        }

        public static Task CallKeyEventFunction(string key, string functionName)
        {
            if (key.Length == 1 && char.IsLetter(key[0]))
            {
                key = key.ToLower();
            }

            var executor = Interop.ServiceProvider.GetRequiredService<IExecutor>();

            if (executor.ProgramState.IsFunctionDefined(functionName))
            {
                var functionStatement = new FunctionCallStatement(
                    new Parser.Expressions.FunctionExpression(new Token(TokenType.Identifier, functionName),
                    new List<IExpression>()
                    {
                        new ValueExpression(new Token(TokenType.String, key))
                    }));

                executor.CreateNewContext(new List<IStatement>() { functionStatement });
                executor.ExecuteAll();
                executor.LeaveContext();
            }

            return Task.FromResult(key);
        }

    }
}
