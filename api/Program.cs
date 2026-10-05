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
    client.DefaultRequestHeaders.Add("x-apisports-key", builder.Configuration["ApiSportsKey"] ?? throw new InvalidOperationException("Configure ApiSportsKey in wwwroot/appsettings.json."));
    return client;
});
await builder.Build().RunAsync();