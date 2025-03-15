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
    client.DefaultRequestHeaders.Add("x-apisports-key", "6d1f3d56ed9f6d42d0545f19dd930c6c");
    return client;
});
await builder.Build().RunAsync();