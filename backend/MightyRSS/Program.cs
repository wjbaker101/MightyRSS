using MightyRSS.Setup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.AddSettings();

services.AddDependencies();
services.AddHostedServices();
services.AddControllers();
services.AddFrontend();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.UseFrontend();

app.Run();