using FIAP.Contacts.Application.Contacts.Models;
using FIAP.MessageBus;
using FIAP.Registration.Application.Commands;
using FIAP.Registration.Application.Handlers;
using FIAP.SharedKernel.Mediator;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Registration API",
        Version = "v1"
    });
}); 

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<CreateContactCommand, FluentValidation.Results.ValidationResult>, CreateContactCommandHandler>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RABBITMQ_HOST"], config =>
        {
            config.Username(builder.Configuration["RABBITMQ_USER"] ?? string.Empty);
            config.Password(builder.Configuration["RABBITMQ_PASSWORD"] ?? string.Empty);
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081);
});

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Registration API v1");
    });
}

app.UseHttpsRedirection();
app.UseHttpMetrics();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapMetrics());
app.UseHealthChecks("/health");

var endpointGroup = app
    .MapGroup("api/contacts");

endpointGroup.MapPost(string.Empty, async (CreateContactInput request, IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand(new CreateContactCommand
    {
        Email = request.Email,
        Name = request.Name,
        PhoneCode = request.PhoneCode,
        PhoneNumber = request.PhoneNumber
    });

    if (result.IsValid)
        return Results.Accepted(value: "Contanct is being created...");

    return Results.BadRequest(result.Errors);
})
.WithTags("Contacts")
.WithName("Create Contact")
.Produces<Accepted>()
.Produces<BadRequest>();

app.Run();

public partial class RegistrationProgram { }
