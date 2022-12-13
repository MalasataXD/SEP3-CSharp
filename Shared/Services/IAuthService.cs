using Shared.DTOs;
using Shared.Models;

namespace Shared.Services;

public interface IAuthService
{
    Task<User> GetUser(string mail);
    Task RegisterUser(UserLoginCreationDto user);
    Task<User> ValidateUser(string mail, string password);
}