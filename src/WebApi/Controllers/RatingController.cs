using Application.Ratings.Commands.CreateRating;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class RatingController : BaseController
{
    [HttpPost]
    public async Task Post([FromBody] CreateRatingCommand command) => await Mediator.Send(command);
}
