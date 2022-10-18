using Shared.Models;

namespace Shared.Services;

public class AuthService : IAuthService
{
    // TODO: Finish AuthService!
    public Task<User> GetUser(string mail, string password)
    {
        throw new NotImplementedException();
    }

    public Task RegisterUser(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> ValidateUser(string mail, string password)
    {
        throw new NotImplementedException();
    }
}