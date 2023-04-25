namespace Application.Common.Interfaces;

public interface IUserService
{
    Task<string> AuthenticateAsync(string email, string password);
    Task<string> CreateUserAsync(string email, string password, string firstName, string lastName);
    Task<int> GetUserIdAsync();
}
