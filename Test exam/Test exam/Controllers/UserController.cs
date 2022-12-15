using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Test_exam.Models;

namespace Test_exam.Controllers;
[Route("/api/[controller]")]
[ApiController]
public class UserController
{
    private readonly IConfiguration _configuration;

    public UserController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [HttpPost]
    public JsonResult Post(User user)
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
                myCommand.Parameters.AddWithValue("@Password", user.Password);
                myCommand.Parameters.AddWithValue("@email", user.Email);
                
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                mycon.Close();
            }
        }
        return new JsonResult("Added Successfully");
    }
}