using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<string>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
        {
            private readonly IUserService _userService;

            public CreateUserCommandHandler(IUserService userService)
            {
                _userService = userService;
            }

            public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken) =>
                await _userService.CreateUserAsync(
                    email: request.Email,
                    password: request.Password,
                    firstName: request.FirstName,
                    lastName: request.LastName
                );
        }
    }
}