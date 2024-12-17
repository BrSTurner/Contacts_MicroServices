using FIAP.Contacts.Application.Contacts.Models;
using FIAP.MessageBus;
using FIAP.Registration.Application.Commands;
using FIAP.Registration.Application.Handlers;
using FIAP.SharedKernel.Mediator;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<CreateContactCommand, FluentValidation.Results.ValidationResult>, CreateContactCommandHandler>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", config =>
        {
            config.Username("guest");
            config.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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