using FIAP.DatabaseManagement.Contacts.Queries;
using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.DatabaseManagement.Context;
using FIAP.DatabaseManagement.Extensions;
using FIAP.DatabaseManagement.UoW;
using FIAP.DatabaseManagement.WS.Contacts.Consumers;
using FIAP.DatabaseManagement.WS.Contacts.Workers;
using FIAP.MessageBus;
using FIAP.SharedKernel.UoW;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PersistContactConsumer>();
    x.AddConsumer<QueryByEmailOrPhoneConsumer>();
    x.AddConsumer<QueryByIdConsumer>();
    x.AddConsumer<DeleteContactConsumer>();

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

builder.Services.AddSingleton<IMessageBus, MessageBus>();

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IContactQueries, ContactQueries>();
builder.Services.AddDbContext<FIAPContext>(c => c.UseInMemoryDatabase("FIAP_Contacts"));
builder.Services.AddInfrastructure(builder.Configuration);
//builder.Services.AddDbContext<FIAPContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHostedService<PersistanceWorker>();

var host = builder.Build();
host.Run();
