using MiNET.AspNet;
using MiNET.AspNet.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(options =>
{
	options.ClearProviders();
	options.AddLog4Net("log4net.xml");
});

builder.Services.AddStartupTask<MinetStartupTask>();
builder.Services.AddShutdownTask<MinetStartupTask>();

WebApplication app = builder.Build();

app.Lifetime.ApplicationStopped.Register(OnShutdown);

await app.RunWithTasksAsync();

async void OnShutdown()
{
	await app.ShutdownWithTasksAsync();
}