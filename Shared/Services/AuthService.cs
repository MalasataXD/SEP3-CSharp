using Shared.DTOs;
using Shared.FileIO.DAOs;
using Shared.Models;

namespace Shared.Services;

public class AuthService : IAuthService
{
    // # Fields
    private readonly IUserLoginDao _userLoginDao;
    
    // ¤ Constructor
    public AuthService(IUserLoginDao userLoginDao)
    {
        _userLoginDao = userLoginDao;
    }

    // ¤ Create new User
    public async Task RegisterUser(UserLoginCreationDto user)
    {
        User? existing = await _userLoginDao.GetByMail(user.Mail);
        if (existing != null)
        {
            throw new Exception("User already exists!");
        }

        User toCreate = new User()
        {
            Mail = user.Mail,
            Password = user.PassWord,
            Role = user.Role,
            WorkerId = user.WorkerId
        };

        await _userLoginDao.CreateAsync(toCreate);
    }
    
    public async Task<User> GetUser(string mail)
    {
        User? user = await _userLoginDao.GetByMail(mail);
        
        if (user == null)
        {
            throw new Exception("Could not find user!");
        }

        return user;
    }
    
    public async Task<User> ValidateUser(string mail, string password)
    {
        User? existing = await _userLoginDao.GetByMail(mail);

        if (existing == null)
        {
            throw new Exception("User not found!");
        }


        if (!existing.Password.Equals(password, StringComparison.Ordinal))
        {
            throw new Exception("Password mismatch!");
        }

        return existing;
    }
}