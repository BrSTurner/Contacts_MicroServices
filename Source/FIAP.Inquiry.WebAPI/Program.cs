using FIAP.Inquiry.Application.Commands;
using FIAP.Inquiry.Application.Handlers;
using FIAP.MessageBus;
using FIAP.SharedKernel.Entities;
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
builder.Services.AddScoped<IRequestHandler<InquiryContactByPhoneCodeCommand, List<Contact?>>, InquiryContactByPhoneCodeCommandHandler>();
builder.Services.AddScoped<IRequestHandler<InquiryAllContactsCommand, List<Contact?>>, InquiryAllContactsCommandHandler>();
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

endpointGroup.MapGet("/{phoneCode:int}", async (int phoneCode, IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand<InquiryContactByPhoneCodeCommand, List<Contact?>>(new InquiryContactByPhoneCodeCommand
    {
        PhoneCode = phoneCode,
    });

    return Results.Ok(result);
})
.WithTags("Contacts")
.WithName("Get Contact By Phone Code")
.Produces<Accepted>()
.Produces<BadRequest>();

endpointGroup.MapGet(string.Empty, async (IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand<InquiryAllContactsCommand, List<Contact?>>(new InquiryAllContactsCommand {    });

    return Results.Ok(result);
})
.WithTags("Contacts")
.WithName("Get All Contacts")
.Produces<Accepted>()
.Produces<NoContent>();

app.Run();

public partial class Program { }