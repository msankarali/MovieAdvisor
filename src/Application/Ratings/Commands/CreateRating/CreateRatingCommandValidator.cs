using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Ratings.Commands.CreateRating;

public class CreateRatingCommandValidator : AbstractValidator<CreateRatingCommand>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;

    public CreateRatingCommandValidator(IDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;


        RuleFor(r => r.MovieId).MustAsync(async (movieId, cancellationToken) =>
        {
            bool exists = await _dbContext.Set<Rating>()
                                          .Where(m => m.Id == movieId)
                                          .AnyAsync(cancellationToken);

            return exists;
        }).WithMessage("Movie not found!");

        RuleFor(r => r.MovieId).MustAsync(async (movieId, cancellationToken) =>
        {
            int userId = await _userService.GetUserIdAsync();

            bool exists = await _dbContext.Set<Rating>()
                                          .Where(m => m.MovieId == movieId && m.UserId == userId)
                                          .AnyAsync(cancellationToken);

            return !exists;
        }).WithMessage("You have already rated for this movie!");

        RuleFor(r => r.Comment).MaximumLength(5000).WithMessage("Comment must not be greater then 5000 characters");
    }
}
