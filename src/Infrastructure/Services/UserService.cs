using Application.Common.Interfaces;
using Application.Common.Models;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    public Task<(Result result, int userId)> CreateUserAsync(string firstName, string lastName, string email, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetUserId()
    {
        return 1;
    }
}