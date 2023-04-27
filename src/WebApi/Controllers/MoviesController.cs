using Application.Common.Models;
using Application.Movies.Commands.RecommendMovie;
using Application.Movies.Queries.GetMovieDetailsById;
using Application.Movies.Queries.GetPagedMovies;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class MoviesController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<DataResult<PagedList<MovieDto>>>> Get([FromQuery] GetPagedMoviesQuery query) => await Mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<DataResult<MovieDetailsDto>>> Get([FromRoute] int id) => await Mediator.Send(new GetMovieDetailsByIdQuery { MovieId = id });

    [HttpPost("{id}/recommend")]
    public async Task<ActionResult<Result>> Post([FromRoute] int id, [FromBody] RecommendMovieCommand command)
    {
        command.MovieId = id;
        Result result = await Mediator.Send(command);
        return result;
    }
}
