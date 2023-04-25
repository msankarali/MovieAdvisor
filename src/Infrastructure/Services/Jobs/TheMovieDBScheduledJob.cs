using Application.Common.Interfaces;
using Application.Common.Models.Cache;
using Domain.Entities;

namespace Infrastructure.Services.Jobs;

public class TheMovieDBScheduledJob
{
    private readonly IMovieDataCollectorIntegrationService _movieDataCollectorIntegrationService;
    private readonly IDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private readonly IJobSchedulerService _jobSchedulerService;

    public TheMovieDBScheduledJob(IMovieDataCollectorIntegrationService movieDataCollectorIntegrationService,
                                  IDbContext dbContext,
                                  ICacheService cacheService,
                                  IJobSchedulerService jobSchedulerService)
    {
        _movieDataCollectorIntegrationService = movieDataCollectorIntegrationService;
        _dbContext = dbContext;
        _cacheService = cacheService;
        _jobSchedulerService = jobSchedulerService;
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

        _jobSchedulerService.ScheduleFireAndForgetJob<TheMovieDBFireAndForgetJob>(
            func: x => x.ExecuteAndForgetMe("some data was passing by the memory..."));
    }
}
