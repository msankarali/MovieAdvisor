using System.Data;
using Application.Common.Interfaces;
using Application.Common.Models.Mail;
using Application.Common.Models.User;
using Application.Movies.Commands.RecommendMovie;
using Hangfire;
using Infrastructure.FilmDataCollectorIntegrationService;
using Infrastructure.MessageBroker;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Services.Jobs;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            
            services.AddAuthentication()
                    .AddJwtBearer()
                    .AddOpenIdConnect(options =>
                    {
                        var auth0Settings = configuration.GetSection(nameof(Auth0Settings)).Get<Auth0Settings>();

                        options.Authority = $"https://{auth0Settings.Domain}";
                        options.ClientId = auth0Settings.ClientId;
                        options.ClientSecret = auth0Settings.ClientSecret;
                        options.ResponseType = auth0Settings.ResponseType;
                        options.CallbackPath = new PathString(auth0Settings.CallbackPath);
                        options.SignedOutCallbackPath = new PathString(auth0Settings.SignedOutCallbackPath);
                        options.RemoteSignOutPath = new PathString(auth0Settings.RemoteSignOutPath);
                        options.SaveTokens = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = "name",
                            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                        };
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

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            services.AddScoped<IUserService, UserService>();

            services.AddAuthorization();

            services.AddTransient<IEventBus, EventBus>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
