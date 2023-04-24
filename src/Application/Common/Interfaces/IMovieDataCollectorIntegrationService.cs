using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IMovieDataCollectorIntegrationService
{
    Task<List<Movie>> Collect();
}
