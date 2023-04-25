using Application;
using Hangfire;
using Infrastructure;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplicationServices(builder.Configuration)
                    .AddInfrastructureServices(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
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
        app.UseHangfireDashboard();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}