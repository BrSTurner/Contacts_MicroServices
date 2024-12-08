using FIAP.Contacts.Application.Contacts.Models;
using FIAP.MessageBus;
using FIAP.Modification.Application.Commands;
using FIAP.Modification.Application.Handlers;
using FIAP.SharedKernel.DTO;
using FIAP.SharedKernel.Mediator;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var endpointGroup = app
    .MapGroup("api/contacts");

endpointGroup.MapPut("/{contactId:guid}", async (Guid contactId, UpdateContactInput contact, IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand(new UpdateContactCommand
    {
        ContactId = contactId,
        Contact = new ContactDTO
        {
            Id = contactId,
            Email = contact.Email,
            Name = contact.Name,
            PhoneCode = contact.PhoneCode,
            PhoneNumber = contact.PhoneNumber,
        }
    });

    if (result.IsValid)
        return Results.Accepted(value: "Contanct is being updated...");


    return Results.BadRequest(result.Errors);
})
.WithTags("Contacts")
.WithName("Update Contact")
.Produces<Accepted>()
.Produces<BadRequest>();

//endpointGroup.MapDelete("{id:guid}", async (Guid id, IMediatorHandler mediator) =>
//{
//    var result = await mediator.SendCommand(new DeleteContactCommand
//    {
//        ContactId = id
//    });

//    if (result.IsValid)
//        return Results.Accepted(value: "Contanct is being deleted...");

//    return Results.BadRequest(result.Errors);
//})
//.WithTags("Contacts")
//.WithName("Delete Contact")
//.Produces<Accepted>()
//.Produces<BadRequest>();


app.Run();

public partial class Program { }