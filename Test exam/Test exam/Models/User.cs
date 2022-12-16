using System.IdentityModel.Tokens.Jwt;

namespace Test_exam.Models;

public class User
{
    public int Id { get; set; }
    public string Username {get; set; }
    public string Password { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? Role { get; set; }
}