using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Test_exam.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Test_exam.Services;
public interface IUserService
{
    bool VerifyUsername(string username);
    bool VerifyEmail(string email);
    User VerifyLogin(User user);
    bool SetUserRole(string username);
}

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool VerifyUsername(string username)
    {
        
        var query = @"SELECT * FROM users WHERE username = @username";
        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
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
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
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
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
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
            var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
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
                return null;
            }

            var userLogin = new User
            {
                Id = table.Rows[0].Field<int>("id"),
                Username = table.Rows[0].Field<string>("username"),
                Email = table.Rows[0].Field<string>("email"),
            };
            var pwFromDb = table.Rows[0].Field<string>("password");
            var verified = BCrypt.Net.BCrypt.Verify(user.Password, pwFromDb);

            if (verified)
            {
                query = "SELECT * FROM user_roles WHERE userId = @id";
                var roleTable = new DataTable();
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand command = new MySqlCommand(query, mycon))
                    {
                        command.Parameters.AddWithValue("@id", userLogin.Id);
                        reader = command.ExecuteReader();
                        roleTable.Load(reader);
                        mycon.Close();
                    }
                }

                var role = roleTable.Rows[0].Field<int>("roleId");
                switch (role)
                {
                    case 1: userLogin.Role = "ROLE_USER";
                        break;
                    case 2: userLogin.Role = "ROLE_STAFF";
                        break;
                    case 3: userLogin.Role = "ROLE_ADMIN";
                        break;
                    
                }
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", userLogin.Id.ToString()),
                    new Claim("UserName", userLogin.Username),
                    new Claim("Email", userLogin.Email)
                };
                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(1440),
                    signingCredentials: signIn);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                userLogin.Token = tokenString;
                
                //userLogin.Token = token;
                
                
                return userLogin;
            }

            return null;
    }
}