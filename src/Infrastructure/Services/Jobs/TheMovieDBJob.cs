using System.Diagnostics;
using Application.Common.Interfaces;
using Application.Common.Models.Cache;
using Domain.Entities;

namespace Infrastructure.Services.Jobs;

public class TheMovieDBJob : IJob
{
    private readonly IMovieDataCollectorIntegrationService _movieDataCollectorIntegrationService;
    private readonly IDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public TheMovieDBJob(IMovieDataCollectorIntegrationService movieDataCollectorIntegrationService,
                         IDbContext dbContext,
                         ICacheService cacheService)
    {
        _movieDataCollectorIntegrationService = movieDataCollectorIntegrationService;
        _dbContext = dbContext;
        _cacheService = cacheService;
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
                _cacheService.RemoveByPattern(CachePatterns.Movies.PagedMovies.Pattern);
            }
        }
    }
}
