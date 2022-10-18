using Shared.Models;

namespace Shared.Services;

public interface IAuthService
{
    Task<User> GetUser(string mail, string password);
    Task RegisterUser(User user);
    Task<User> ValidateUser(string mail, string password);
}