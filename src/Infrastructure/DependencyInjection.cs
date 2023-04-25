using Application.Common.Interfaces;
using Application.Common.Jobs;
using Application.Common.Models.Mail;
using Application.Common.Security;
using Application.Movies.Commands.RecommendMovie;
using Hangfire;
using Infrastructure.FilmDataCollectorIntegrationService;
using Infrastructure.MessageBroker;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Infrastructure.Services;
using Infrastructure.Services.Jobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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

            services.AddSingleton<IJobSchedulerService, HangfireJobSchedulerService>();
            var jobSchedulerService = services.BuildServiceProvider().GetRequiredService<IJobSchedulerService>();

            services.AddTransient<ITheMovieDBContinuationJob, TheMovieDBDelayedJob>();

            jobSchedulerService.ScheduleRecurringJob<TheMovieDBScheduledJob>(x => x.Execute(), "* * * * *");

            jobSchedulerService.ScheduleContinuationJob<TheMovieDBScheduledJob, TheMovieDBContinuationJob>(
                func: x => x.Execute(),
                continuationFunc: x => x.ExecuteContinuation());

            jobSchedulerService.ScheduleDelayedJob<TheMovieDBDelayedJob>(
                func: x => x.Trigger(),
                delay: TimeSpan.FromSeconds(10));

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie Advisor Web API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });

            services.AddSingleton<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();

            services.AddTransient<IEventBus, EventBus>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
