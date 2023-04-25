using System.Security.Authentication;
using Application.Common.Interfaces;
using Application.Common.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly Auth0Settings _auth0Settings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _auth0Settings = configuration.GetSection(nameof(Auth0Settings)).Get<Auth0Settings>();
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_auth0Settings.Domain}/oauth/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "http://auth0.com/oauth/grant-type/password-realm" },
                { "client_id", _auth0Settings.ClientId },
                { "client_secret", _auth0Settings.ClientSecret },
                { "username", username },
                { "password", password },
                { "realm", "Username-Password-Authentication" },
            })
        };

        var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new AuthenticationException(responseContent);
        }

        var token = JsonConvert.DeserializeObject<Auth0TokenResponse>(responseContent);

        // await _httpContextAccessor.HttpContext.SignInAsync();

        return token.AccessToken;
    }

    public Task<bool> CreateUserAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetUserId()
    {
        throw new NotImplementedException();
    }

    public Task<string> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task RevokeTokenAsync(string token, string clientId)
    {
        throw new NotImplementedException();
    }
}