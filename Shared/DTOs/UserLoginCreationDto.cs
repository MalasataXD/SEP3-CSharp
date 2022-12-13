namespace Shared.DTOs;

public class UserLoginCreationDto
{
    public int WorkerId { get; init; }
    public string Mail { get; init; }
    public string PassWord { get; init; }
    public string Role { get; set; } = "user";
}