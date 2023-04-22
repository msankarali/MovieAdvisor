using Application.Common.Interfaces;
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

        public CreateRatingCommandHandler(IDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
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

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
