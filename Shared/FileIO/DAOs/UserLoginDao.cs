using Shared.Models;

namespace Shared.FileIO.DAOs;

public class UserLoginDao : IUserLoginDao
{
    // # Fields
    private readonly LoginContext _context;

    // ¤ Constructor
    public UserLoginDao(LoginContext context)
    {
        this._context = context;
    }
    // ¤ Create method
    public Task<User> CreateAsync(User user)
    {
        int userId = 1;
        // # Check if other users exists and count them.
        if (_context.Users.Any())
        {
            userId = _context.Users.Max(u => u.WorkerId);
            userId++;
        }

        user.WorkerId = userId;
        
        // # Add user to list of users.
        _context.Users.Add(user);
        // # Save changes in database.
        _context.SaveChanges();

        return Task.FromResult(user);
    }

    public Task<User?> GetByMail(string mail)
    {
        User? existing = _context.Users.FirstOrDefault(u 
            => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase));
        
        return Task.FromResult(existing);
    }
}