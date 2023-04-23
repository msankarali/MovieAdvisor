using Application.Common.Models;
using Application.Movies.Commands.RecommendMovie;
using Application.Movies.Queries.GetMovieDetailsById;
using Application.Movies.Queries.GetPagedMovies;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class MoviesController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<MovieDto>>> Get([FromQuery] GetPagedMoviesQuery query)
    {
        PagedList<MovieDto> result = await Mediator.Send(query);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDetailsDto>> Get([FromRoute] int id)
    {
        MovieDetailsDto result = await Mediator.Send(new GetMovieDetailsByIdQuery { MovieId = id });
        return result;
    }

    [HttpPost("{id}/recommend")]
    public async Task<ActionResult<Result>> Post([FromRoute] int id, [FromQuery] string email)
    {
        Result result = await Mediator.Send(new RecommendMovieCommand { MovieId = id, Email = email });
        return result;
    }
}