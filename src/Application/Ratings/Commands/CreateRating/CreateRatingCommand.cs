using Application.Common.Interfaces;
using Application.Common.Jobs;
using Application.Common.Models;
using Application.Common.Models.Cache;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Ratings.Commands.CreateRating;


[Authorized]
public class CreateRatingCommand : IRequest<Result>
{
    public int MovieId { get; init; }
    public int Score { get; init; }
    public string? Comment { get; init; }

    public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        private readonly IJobSchedulerService _jobSchedulerService;

        public CreateRatingCommandHandler(IDbContext dbContext, IUserService userService, ICacheService cacheService, IJobSchedulerService jobSchedulerService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _cacheService = cacheService;
            _jobSchedulerService = jobSchedulerService;
        }

        public async Task<Result> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            int userId = await _userService.GetUserIdAsync();

            var ratingToAdd = Rating.CreateRating(
                movieId: request.MovieId,
                userId: userId,
                score: request.Score,
                comment: request.Comment
            );

            await _dbContext.Set<Rating>().AddAsync(ratingToAdd);

            _jobSchedulerService.ScheduleFireAndForgetJob<ITheMovieDBContinuationJob>(x => x.Trigger());

            if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
            {
                await _cacheService.RemoveAsync(CachePatterns.Movies.MovieDetails.GetMovieDetailsById(ratingToAdd.MovieId));
                return Result.Success($"You have successfully rated the movie!");
            }

            return Result.Error("There occured an error while rating the movie!");
        }
    }
}
