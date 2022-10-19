using Shared.Models;

namespace Shared.FileIO.DAOs;

public interface IUserLoginDao
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByMail(string mail);
}