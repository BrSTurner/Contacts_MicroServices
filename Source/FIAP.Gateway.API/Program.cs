using Ocelot.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOcelot();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8500);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();

