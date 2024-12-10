using FIAP.Inquiry.Application.Commands;
using FIAP.Inquiry.Application.Handlers;
using FIAP.MessageBus;
using FIAP.SharedKernel.Mediator;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<InquiryContactCommand, FluentValidation.Results.ValidationResult>, InquiryContactCommandHandler>();
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

endpointGroup.MapPut("/{phoneCode:int}", async (int phoneCode, IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand(new InquiryContactCommand
    {
        PhoneCode = phoneCode,
    });

    if (result.IsValid)
        return Results.Accepted(value: "contact is being picked up...");


    return Results.BadRequest(result.Errors);
})
.WithTags("Contacts")
.WithName("Update Contact")
.Produces<Accepted>()
.Produces<BadRequest>();

app.Run();

public partial class Program { }