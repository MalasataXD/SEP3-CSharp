namespace Shared.DTOs;

public class UserLoginDto
{
    // # Fields
    // NOTE: More can be added, if more information is needed to login.
    public string Mail { get; init; }
    public string Password { get; init; }
}