namespace Application.Common.Models.Cache;

public abstract class CachePatterns
{
    public abstract class Movies
    {
        public abstract class PagedMovies
        {
            public const string Pattern = "movies-";
        }

        public abstract class MovieDetails
        {
            public const string Pattern = "movie-";
            public static string GetMovieDetailsById(int movieId) => $"{Pattern}|movieId:{movieId}";
        }
    }
}