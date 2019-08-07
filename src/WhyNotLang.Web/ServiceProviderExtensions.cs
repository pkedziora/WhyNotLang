using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter.Builtin;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Web
{
    public static class ServiceProviderExtensions
    {
        public static void AddWebInputOutput(this IServiceProvider serviceProvider, Microsoft.JSInterop.IJSRuntime jsRuntime)
        {
            var functionCollection = serviceProvider.GetRequiredService<IBuiltinFunctionCollection>();
            functionCollection.Add("Log",
                async arguments =>
                {
                    var str = arguments.Single();
                    if (str.Type != ExpressionValueTypes.String)
                    {
                        throw new Exception("String expected");
                    }

                    await jsRuntime.InvokeAsync<string>("console.log", str.Value);

                    return await Task.FromResult(ExpressionValue.Empty);
                });
        }
    }
}