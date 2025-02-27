using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using footApi.Services;
using footApi;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<FootService>();

builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://v3.football.api-sports.io/") };
    client.DefaultRequestHeaders.Add("x-apisports-key", "0e70e8e8fdfe76a43706cc4ca7e9d8c5");
    return client;
});
await builder.Build().RunAsync();