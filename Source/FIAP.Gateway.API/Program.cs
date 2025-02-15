using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOcelot();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gateway API",
        Version = "v1",
        Description = "Aggregated Swagger API Gateway",
    });
});


builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8500);
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/aggregated", "Aggregated Gateway API v1");
});


app.MapGet("/swagger/aggregated", async (IHttpClientFactory httpClientFactory) =>
{
    var downstreamServices = new List<string>
    {
        "http://registration:8081/swagger/v1/swagger.json",
        "http://termination:8081/swagger/v1/swagger.json",
        "http://inquiry:8081/swagger/v1/swagger.json",
        "http://modification:8081/swagger/v1/swagger.json"
    };

    var httpClient = httpClientFactory.CreateClient();
    var swaggerDocs = new List<JsonElement>();

    foreach (var url in downstreamServices)
    {
        try
        {
            var response = await httpClient.GetStringAsync(url);
            var document = JsonDocument.Parse(response);
            swaggerDocs.Add(document.RootElement);
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"Failed to fetch Swagger document from {url}: {ex.Message}");
        }
    }

    var aggregatedSwagger = new
    {
        openapi = "3.0.1",
        info = new
        {
            title = "Aggregated Gateway API",
            version = "v1"
        },
        paths = new Dictionary<string, object>(),
        components = new { schemas = new Dictionary<string, object>() }
    };

    foreach (var doc in swaggerDocs)
    {
        var paths = doc.GetProperty("paths");
        foreach (var path in paths.EnumerateObject())
        {
            aggregatedSwagger.paths[path.Name] = path.Value;
        }
    }

    return Results.Json(aggregatedSwagger);
});

app.Run();

