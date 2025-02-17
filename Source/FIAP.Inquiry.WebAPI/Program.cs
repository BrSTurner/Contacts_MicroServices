using FIAP.Inquiry.Application.Commands;
using FIAP.Inquiry.Application.Handlers;
using FIAP.MessageBus;
using FIAP.SharedKernel.DTO;
using FIAP.SharedKernel.Entities;
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
        Title = "Inquiry API",
        Version = "v1"
    });
});
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<InquiryContactByPhoneCodeCommand, List<ContactDTO>>, InquiryContactByPhoneCodeCommandHandler>();
builder.Services.AddScoped<IRequestHandler<InquiryAllContactsCommand, List<Contact?>>, InquiryAllContactsCommandHandler>();
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inquiry API v1");
    });
}

app.UseHttpsRedirection();
app.UseHttpMetrics();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapMetrics());
app.UseHealthChecks("/health");

var endpointGroup = app
    .MapGroup("api/contacts");

endpointGroup.MapGet("/{phoneCode:int}", async (int phoneCode, IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand<InquiryContactByPhoneCodeCommand, List<ContactDTO>>(new InquiryContactByPhoneCodeCommand
    {
        PhoneCode = phoneCode,
    });

    if (result == null || result.Count <= 0)
        return Results.NoContent();

    return Results.Ok(result);
})
.WithTags("Contacts")
.WithName("Get Contact By Phone Code")
.Produces<Ok>()
.Produces<NoContent>()
.Produces<BadRequest>();

endpointGroup.MapGet(string.Empty, async (IMediatorHandler mediator) =>
{
    var result = await mediator.SendCommand<InquiryAllContactsCommand, List<Contact?>>(new InquiryAllContactsCommand {    });

    if (result == null || result.Count <= 0)
        return Results.NoContent();

    return Results.Ok(result.Select(c => new ContactDTO
    {
        Id = c.Id,
        Email = c.Email.Address,
        PhoneCode = c.PhoneNumber.Code,
        PhoneNumber = c.PhoneNumber.Number,
        Name = c.Name
    }));
})
.WithTags("Contacts")
.WithName("Get All Contacts")
.Produces<Ok>()
.Produces<NoContent>();

app.Run();

public partial class InquiryProgram { }