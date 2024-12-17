using FIAP.DatabaseManagement.Extensions;
using FIAP.DatabaseManagement.WS.Contacts.Consumers;
using FIAP.DatabaseManagement.WS.Contacts.Workers;
using FIAP.MessageBus;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PersistContactConsumer>();
    x.AddConsumer<QueryByEmailOrPhoneConsumer>();
    x.AddConsumer<QueryByPhoneCodeConsumer>();
    x.AddConsumer<UpdateContactConsumer>();
    x.AddConsumer<QueryByIdConsumer>();
    x.AddConsumer<GetAllContactsConsumer>();
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
builder.Services.AddInfrastructure(builder.Configuration, true);
builder.Services.AddHostedService<PersistanceWorker>();

var host = builder.Build();
host.Run();
