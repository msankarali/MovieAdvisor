using Application.Common.Interfaces;
using Application.Common.Models.Cache;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Ratings.Commands.CreateRating;

public class CreateRatingCommand : IRequest<Unit>
{
    public int MovieId { get; init; }
    public int Score { get; init; }
    public string? Comment { get; init; }

    public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;

        public CreateRatingCommandHandler(IDbContext dbContext, IUserService userService, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _cacheService = cacheService;
        }

        public async Task<Unit> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            int userId = await _userService.GetUserId();

            var ratingToAdd = Rating.CreateRating(
                movieId: request.MovieId,
                userId: userId,
                score: request.Score,
                comment: request.Comment
            );

            await _dbContext.Set<Rating>().AddAsync(ratingToAdd);

            if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
            {
                await _cacheService.RemoveAsync(CachePatterns.Movies.MovieDetails.GetMovieDetailsById(ratingToAdd.MovieId));
            }

            return Unit.Value;
        }
    }
}
