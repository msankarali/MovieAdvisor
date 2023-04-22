using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IUserService
{
    Task<(Result result, int userId)> CreateUserAsync(string firstName, string lastName, string email, string password);
    Task<int> GetUserId();
}
