using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Test_exam.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Test_exam.Services;
public interface IUserService
{
    bool VerifyUsername(string username);
    bool VerifyEmail(string email);
    User VerifyLogin(User user);
    bool SetUserRole(string username);
    User SetRoleAndToken(User user);
    IActionResult CreateUser(User user);
}

public class UserService : IUserService
{
    private readonly IConfiguration _configuration = null!;
    private readonly string _connectionString = "Server=localhost;Database=rategame;Username=user;Password=password";
    
    public UserService()
    {
    }

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool VerifyUsername(string username)
    {
        
        var query = @"SELECT * FROM users WHERE username = @username";
        var table = new DataTable();
        var sqlDataSource = _connectionString;//_configuration.GetConnectionString("MySQLCon");
        MySqlDataReader reader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand command = new MySqlCommand(query, mycon))
            {
                command.Parameters.AddWithValue("@username", username);
                reader = command.ExecuteReader();
                table.Load(reader);
                mycon.Close();
            }
        }
        return table.Rows.Count != 1;
    }
    
    public bool VerifyEmail(string email)
    {
        var query = @"SELECT * FROM users WHERE email = @email";
        var table = new DataTable();
        var sqlDataSource = _connectionString;//_configuration.GetConnectionString("MySQLCon");;
        MySqlDataReader reader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand command = new MySqlCommand(query, mycon))
            {
                command.Parameters.AddWithValue("@email", email);
                reader = command.ExecuteReader();
                table.Load(reader);
                mycon.Close();
            }
        }
        return table.Rows.Count != 1;
    }

    public bool SetUserRole(string username)
    {
        var query = @"SELECT * FROM users WHERE username = @username";

        var table = new DataTable();
        var sqlDataSource = _connectionString;//_configuration.GetConnectionString("MySQLCon");
        MySqlDataReader myReader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
            {
                myCommand.Parameters.AddWithValue("@username", username);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                mycon.Close();
            }
            
            var userId = table.Rows[0].Field<int>("id");
            

            if (userId>0)
            {
                var roleQuery = @"INSERT INTO user_roles (userId, roleId) values (@userId, @roleId)";
                var roleTable = new DataTable();
                using (MySqlConnection con = new MySqlConnection(sqlDataSource))
                {
                    con.Open();
                    using (MySqlCommand command = new MySqlCommand(roleQuery, con))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@roleId", 1);
                        myReader = command.ExecuteReader();
                        roleTable.Load(myReader);
                        con.Close();
                    }
                }
                
            }
            return true;
        }
    }

    public User VerifyLogin(User user)
    {
        var query = @"SELECT * FROM users WHERE username = @username";
        var table = new DataTable();
        var sqlDataSource = _connectionString; //_configuration.GetConnectionString("MySQLCon");
        MySqlDataReader reader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand command = new MySqlCommand(query, mycon))
            {
                command.Parameters.AddWithValue("@username", user.Username);
                reader = command.ExecuteReader();
                table.Load(reader);
                mycon.Close();
            }
        }

        if (table.Rows.Count == 0)
        {
            return null!;
        }

        var userLogin = new User
        {
            Id = table.Rows[0].Field<int>("id"),
            Username = table.Rows[0].Field<string>("username")!,
            Email = table.Rows[0].Field<string>("email"),
        };
        var pwFromDb = table.Rows[0].Field<string>("password");
        var verified = BCrypt.Net.BCrypt.Verify(user.Password, pwFromDb);
        if (verified)
        {
            return userLogin;
        }

        return null!;
    }

    public User SetRoleAndToken(User user)
    {
        var userWithToken = new User
        {
            Id = user.Id,
            Username = user.Username,
            Password = user.Password,
            Email = user.Email,
        };
        var sqlDataSource = _connectionString;//_configuration.GetConnectionString("MySQLCon");
        var query = "SELECT * FROM user_roles WHERE userId = @id";
        MySqlDataReader reader;
                var roleTable = new DataTable();
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand command = new MySqlCommand(query, mycon))
                    {
                        command.Parameters.AddWithValue("@id", userWithToken.Id);
                        reader = command.ExecuteReader();
                        roleTable.Load(reader);
                        mycon.Close();
                    }
                }

                var role = roleTable.Rows[0].Field<int>("roleId");

                switch (role)
                {
                    case 1:
                        userWithToken.Role = "ROLE_USER";
                        break;
                    case 2:
                        userWithToken.Role = "ROLE_STAFF";
                        break;
                    case 3: 
                        userWithToken.Role = "ROLE_ADMIN";
                        break;
                }

                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.CurrentCulture)),
                    new Claim("UserId", userWithToken.Id.ToString()),
                    new Claim("UserName", userWithToken.Username),
                    new Claim("Email", userWithToken.Email!)
                };
                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx"));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    "JWTAuthenticationServer",
                    "JWTServicePostmanClient",
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(1440),
                    signingCredentials: signIn);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                userWithToken.Token = tokenString;

                return userWithToken;
    }

    public IActionResult CreateUser(User user)
    {
        try
        {
            if (VerifyUsername(user.Username) && VerifyEmail(user.Email!))
            {
                var query = @"INSERT INTO users (Username, Password, email) 
        values (@Username, @Password, @email)";

                var table = new DataTable();
                var sqlDataSource = _connectionString;//_configuration.GetConnectionString("MySQLCon");
                MySqlDataReader myReader;
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myCommand.Parameters.AddWithValue("@Username", user.Username);
                        myCommand.Parameters.AddWithValue("@Password", BCrypt.Net.BCrypt.HashPassword(user.Password));
                        myCommand.Parameters.AddWithValue("@email", user.Email);
                
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        mycon.Close();
                    }
                }

                if (SetUserRole(user.Username))
                {
                    return new OkResult();
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return new BadRequestResult();
    }

}