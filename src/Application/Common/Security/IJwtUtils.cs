using Domain.Entities;

namespace Application.Common.Security;

public interface IJwtUtils
{
    public string GenerateJwtToken(User user);
    public int ValidateJwtToken(string? token);
}
