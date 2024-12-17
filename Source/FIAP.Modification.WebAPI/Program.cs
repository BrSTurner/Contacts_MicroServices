using FIAP.Contacts.Application.Contacts.Models;
using FIAP.MessageBus;
using FIAP.Modification.Application.Commands;
using FIAP.Modification.Application.Handlers;
using FIAP.SharedKernel.Mediator;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateContactCommand, FluentValidation.Results.ValidationResult>, UpdateContactCommandHandler>();
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
app.UseHttpMetrics();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapMetrics());

var endpointGroup = app
    .MapGroup("api/contacts");

endpointGroup.MapPut("/{contactId:guid}", async (Guid contactId, UpdateContactInput contact, IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand(new UpdateContactCommand
    {
        Id = contactId,
        Email = contact.Email,
        Name = contact.Name,
        PhoneCode = contact.PhoneCode,
        PhoneNumber = contact.PhoneNumber 
    });

    if (result.IsValid)
        return Results.Accepted(value: "Contact is being updated...");


    return Results.BadRequest(result.Errors);
})
.WithTags("Contacts")
.WithName("Update Contact")
.Produces<Accepted>()
.Produces<BadRequest>();

app.Run();

public partial class UpdateProgram { }