using System.Net.Http.Json;
using Application.Common.Interfaces;
using Application.Common.Models.MovieDataCollectorIntegration;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.FilmDataCollectorIntegrationService;

public class TheMovieDbDataCollectorIntegrationService : IMovieDataCollectorIntegrationService
{
    private readonly TheMovieDbSettings _theMovieDbSettings;

    public TheMovieDbDataCollectorIntegrationService(IConfiguration configuration)
    {
        _theMovieDbSettings = configuration.GetSection(nameof(TheMovieDbSettings)).Get<TheMovieDbSettings>();
    }

    public async Task<List<Movie>> Collect()
    {
        using var client = new HttpClient();

        var data = await client.GetFromJsonAsync<TMDBResponseModel>($"https://api.themoviedb.org/3/movie/latest?api_key={_theMovieDbSettings.APIKey_v3}&language=en-US");

        var movieList = new List<Movie>();
        if (data != null)
            for (int i = 0; i < data?.Results?.Count; i++)
            {
                var movieFromTMDB = data.Results[i];
                var film = new Movie
                {
                    Title = movieFromTMDB.Title,
                    Description = movieFromTMDB.Overview,
                    ReleaseDate = DateTime.Parse(movieFromTMDB.ReleaseDate)
                };

                movieList.Add(film);
            }

        return movieList;
    }
}
