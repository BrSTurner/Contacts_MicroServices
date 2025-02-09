using FIAP.DatabaseManagement.Extensions;
using FIAP.DatabaseManagement.WS.Contacts.Consumers;
using FIAP.DatabaseManagement.WS.Contacts.Workers;
using FIAP.MessageBus;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddEnvironmentVariables();

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
        cfg.Host(builder.Configuration["RABBITMQ_HOST"], config =>
        {
            config.Username(builder.Configuration.GetSection("RabbitMQ").GetValue<string>("User") ?? string.Empty);
            config.Password(builder.Configuration.GetSection("RabbitMQ").GetValue<string>("Password") ?? string.Empty);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<IMessageBus, MessageBus>();
builder.Services.AddInfrastructure(builder.Configuration, true);
builder.Services.AddHostedService<PersistanceWorker>();

var host = builder.Build();
host.Run();
