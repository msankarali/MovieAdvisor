using Application.Common.Interfaces;
using Application.Common.Models.Mail;
using Application.Movies.Commands.RecommendMovie;
using Hangfire;
using Infrastructure.FilmDataCollectorIntegrationService;
using Infrastructure.MessageBroker;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Services.Jobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MovieAdvisorDbContext>(opt =>
            {
                opt.UseInMemoryDatabase("MovieAdvisorDB");
            });

            services.AddScoped<IDbContext>(provider => provider.GetRequiredService<MovieAdvisorDbContext>());
            services.AddScoped<MovieAdvisorDbContextInitializer>();
            services.AddScoped<IUserService, UserService>();

            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);
            services.AddSingleton<IEmailService, SmtpEmailService>();

            services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingsKey));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddSingleton<ICacheService, RedisCacheService>();

            services.AddScoped<IMovieDataCollectorIntegrationService, TheMovieDbDataCollectorIntegrationService>();

            services.AddHangfire(config => config.UseSqlServerStorage(configuration.GetConnectionString("Hangfire")));
            services.AddHangfireServer();

            services.AddTransient<IJobSchedulerService, HangfireJobSchedulerService>();
            var jobSchedulerService = services.BuildServiceProvider().GetRequiredService<IJobSchedulerService>();
            jobSchedulerService.ScheduleRecurringJob<TheMovieDBJob>("0 0-23 * * *");

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<MovieRecommendedEventConsumer>();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

                    configurator.Host(new Uri(settings.Host), host =>
                    {
                        host.Username(settings.Username);
                        host.Password(settings.Password);
                    });

                    configurator.ReceiveEndpoint("event", e =>
                    {
                        e.ConfigureConsumer<MovieRecommendedEventConsumer>(context);
                    });
                });
            });

            services.AddTransient<IEventBus, EventBus>();

            return services;
        }
    }
}
