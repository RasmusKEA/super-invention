using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Test_exam.Models;
using Test_exam.Services;

namespace Test_exam.Controllers;
[Route("/api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public UserController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }
    [HttpPost("register")]
    public JsonResult Post(User user)
    {
        
        try
        {
            if (_userService.VerifyUsername(user.Username) && _userService.VerifyEmail(user.Email))
            {
                var query = @"INSERT INTO users (Username, Password, email) 
        values (@Username, @Password, @email)";

                var table = new DataTable();
                var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
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

                if (_userService.SetUserRole(user.Username))
                {
                    return new JsonResult("Added Successfully");
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return new JsonResult("User could not be created");
    }

    [HttpPost("login")]
    public JsonResult Login(User user)
    {
        var userLogin =_userService.VerifyLogin(user);
        return new JsonResult(userLogin);
    }
}