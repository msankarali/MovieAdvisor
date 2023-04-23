using System.Diagnostics;
using System.Text.Json;
using Application;
using Application.Common.Interfaces;
using Application.Common.Models.Mail;
using Application.Movies.Commands.RecommendMovie;
using Infrastructure;
using Infrastructure.MessageBroker;
using Infrastructure.Persistence;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplicationServices(builder.Configuration)
                    .AddInfrastructureServices(builder.Configuration);

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