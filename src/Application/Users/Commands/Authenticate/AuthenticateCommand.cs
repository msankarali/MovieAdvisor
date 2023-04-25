using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.Authenticate;

public class AuthenticateCommand : IRequest<string>
{
    public string Email { get; init; }
    public string Password { get; init; }

    public class AuthenticateCommandHander : IRequestHandler<AuthenticateCommand, string>
    {
        private readonly IUserService _userService;

        public AuthenticateCommandHander(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<string> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var token = await _userService.AuthenticateAsync(request.Email, request.Password);
            return token;
        }
    }
}
