using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IDbContext _dbContext;
    private readonly IJwtUtils _jwtUtils;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IDbContext dbContext, IJwtUtils jwtUtils, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _jwtUtils = jwtUtils;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        var user = await _dbContext.Set<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        if (user is null)
        {
            throw new NotFoundException("Wrong email or password!");
        }

        if (!HashingUtils.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            throw new NotFoundException("Wrong email or password!");
        }

        return _jwtUtils.GenerateJwtToken(user);
    }

    public async Task<string> CreateUserAsync(string email, string password, string firstName, string lastName)
    {
        var isUserRegistered = _dbContext.Set<User>().Where(u => u.Email == email).Any();
        if (isUserRegistered)
        {
            throw new NotFoundException("User already exists!");
        }

        HashingUtils.CreatePasswordHash(password, out byte[] passwordHash, out var passwordSalt);

        await _dbContext.Set<User>().AddAsync(new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        });
        await _dbContext.SaveChangesAsync(default);

        return await AuthenticateAsync(email, password);
    }

    public Task<int> GetUserIdAsync()
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = _jwtUtils.ValidateJwtToken(token);
        return Task.FromResult(userId);
    }
}