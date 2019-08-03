using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using WhyNotLang.Interpreter;

namespace WhyNotLang.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWhyNotLang();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            var jsRuntime = app.Services.GetRequiredService<IJSRuntime>();
            app.Services.AddWebInputOutput(jsRuntime);
            app.AddComponent<App>("app");
            Interop.ServiceProvider = app.Services;
        }
    }
}
