using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using WhyNotLang.Blazor;
using WhyNotLang.EmbeddedResources.Reader;
using WhyNotLang.Interpreter;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddWhyNotLang();
builder.Services.AddSampleReader();
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();
var jsRuntime = app.Services.GetRequiredService<IJSRuntime>();
app.Services.AddWebInputOutput(jsRuntime);
Interop.ServiceProvider = app.Services;
await app.RunAsync();

//app.AddComponent<App>("app");


