using FIAP.MessageBus;
using FIAP.SharedKernel.Mediator;
using FIAP.Termination.Application.Commands;
using FIAP.Termination.Application.Handlers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteContactCommand, FluentValidation.Results.ValidationResult>, DeleteContactCommandHandler>();
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

endpointGroup.MapDelete("{id:guid}", async (Guid id, IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand(new DeleteContactCommand
    {
        ContactId = id
    });

    if (result.IsValid)
        return Results.Accepted(value: "Contact is being deleted...");

    return Results.BadRequest(result.Errors);
})
.WithTags("Contacts")
.WithName("Delete Contact")
.Produces<Accepted>()
.Produces<BadRequest>();

app.Run();

public partial class TerminationProgram { }