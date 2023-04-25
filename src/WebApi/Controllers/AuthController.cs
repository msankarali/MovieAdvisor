using Application.Users.Commands.Authenticate;
using Application.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : BaseController
{
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] AuthenticateCommand command) => await Mediator.Send(command);

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] CreateUserCommand command) => await Mediator.Send(command);
}
