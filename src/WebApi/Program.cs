using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.MessageBroker;
using Infrastructure.Persistence;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplicationServices()
                    .AddInfrastructureServices();

    builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection(MessageBrokerSettings.SettingsKey));
    builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
    builder.Services.AddTransient<IEventBus, EventBus>();

    builder.Services.AddMassTransit(busConfigurator =>
    {
        busConfigurator.SetKebabCaseEndpointNameFormatter();

        busConfigurator.UsingRabbitMq((context, configurator) =>
        {
            MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

            configurator.Host(new Uri(settings.Host), host =>
            {
                host.Username(settings.Username);
                host.Password(settings.Password);
            });
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{

    using var scope = app.Services.CreateScope();
    {
        var initializer = scope.ServiceProvider.GetRequiredService<MovieAdvisorDbContextInitializer>();
        await initializer.Seed();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}