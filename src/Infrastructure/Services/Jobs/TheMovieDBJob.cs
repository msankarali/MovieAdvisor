using System.Diagnostics;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services.Jobs;

public class TheMovieDBJob : IJob
{
    private readonly IMovieDataCollectorIntegrationService _movieDataCollectorIntegrationService;
    private readonly IDbContext _dbContext;

    public TheMovieDBJob(IMovieDataCollectorIntegrationService movieDataCollectorIntegrationService, IDbContext dbContext)
    {
        _movieDataCollectorIntegrationService = movieDataCollectorIntegrationService;
        _dbContext = dbContext;
    }
    public void Execute()
    {
        var movies = _movieDataCollectorIntegrationService.Collect().GetAwaiter().GetResult();
        foreach (var movie in movies)
        {
            if (!_dbContext.Set<Movie>().Where(m => m.Title == movie.Title).Any())
            {
                _dbContext.Set<Movie>().Add(movie);
                _dbContext.SaveChangesAsync(default).GetAwaiter().GetResult();
            }
        }
    }
}
