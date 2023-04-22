using Application.Common.Models;
using Application.Movies.Queries.GetMovieDetailsById;
using Application.Movies.Queries.GetPagedMovies;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class MoviesController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<MovieDto>>> Get([FromQuery] GetPagedMoviesQuery query) => await Mediator.Send(query);

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDetailsDto>> Get([FromRoute] int id) => await Mediator.Send(new GetMovieDetailsByIdQuery { MovieId = id });
}