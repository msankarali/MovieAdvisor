using Domain.Common;

namespace Application.Movies.Commands.RecommendMovie;

public class MovieRecommendedEvent : BaseEvent
{
    public MovieRecommendedEvent(string email, int movieId, string movieTitle)
    {
        Email = email;
        MovieId = movieId;
        MovieTitle = movieTitle;
    }

    public string Email { get; }
    public int MovieId { get; }
    public string MovieTitle { get; }
}
