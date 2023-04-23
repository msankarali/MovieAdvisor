using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Movies.Commands.RecommendMovie;

public class RecommendMovieCommand : IRequest<Result>
{
    public int MovieId { get; init; }
    public string Email { get; init; }

    public class RecommendMovieCommandHandler : IRequestHandler<RecommendMovieCommand, Result>
    {
        private readonly IDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public RecommendMovieCommandHandler(IDbContext dbContext, IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public async Task<Result> Handle(RecommendMovieCommand request, CancellationToken cancellationToken)
        {
            var movieToRecommend = await _dbContext.Set<Movie>().Where(m => m.Id == request.MovieId).SingleAsync();

            await _eventBus.PublishAsync(
                new MovieRecommendedEvent(
                    email: request.Email,
                    movieId: movieToRecommend.Id,
                    movieTitle: movieToRecommend.Title),
                    cancellationToken: cancellationToken
                );

            return Result.SuccessOperation();
        }
    }
}
