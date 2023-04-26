namespace Domain.Entities;

public class Rating : BaseEntity
{
    public Rating() { }
    private Rating(int movieId, int userId, int score, string? comment)
    {
        MovieId = movieId;
        UserId = userId;
        Score = score;
        Comment = comment;
    }

    public int Id { get; set; }
    public int MovieId { get; set; }
    public int UserId { get; set; }

    private int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            if (value < 0 || value > 10)
            {
                throw new RatingOutOfBoundsException();
            }
            _score = value;
        }
    }

    public string? Comment { get; set; }

    public virtual User User { get; set; }
    public virtual Movie Movie { get; set; }

    public static Rating CreateRating(int movieId, int userId, int score, string? comment)
    {
        if (score < 0 || score > 10) throw new RatingOutOfBoundsException();

        var rating = new Rating(movieId, userId, score, comment);

        return rating;
    }
}
