using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Test_exam.Models;

namespace Test_exam.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ReviewController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public JsonResult Get()
    {
        var query = @"SELECT * FROM reviews";
        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
        MySqlDataReader reader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand command = new MySqlCommand(query, mycon))
            {
                reader = command.ExecuteReader();
                table.Load(reader);
                mycon.Close();
            }
        }
        return new JsonResult(table);
    }
    
    [HttpGet("{id:int}")]
    public JsonResult GetById(int id)
    {
        var query = @"SELECT * FROM reviews WHERE id = @id";
        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
        MySqlDataReader reader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand command = new MySqlCommand(query, mycon))
            {
                command.Parameters.AddWithValue("@id", id);
                reader = command.ExecuteReader();
                table.Load(reader);
                mycon.Close();
            }
        }
        return new JsonResult(table);
    }
    
    [HttpGet("featured")]
    public JsonResult GetFeatured()
    {
        var query = @"SELECT * FROM reviews WHERE featured = 1";
        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
        MySqlDataReader reader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand command = new MySqlCommand(query, mycon))
            {
                reader = command.ExecuteReader();
                table.Load(reader);
                mycon.Close();
            }
        }
        return new JsonResult(table);
    }
    [Authorize]
    [HttpPost]
    public JsonResult Post(Reviews reviews)
    {
        var query = @"INSERT INTO reviews (idUser, review, title, rating, ratingReasoning, platform, image, featured) 
        values (@idUser, @review, @title, @rating, @ratingReasoning, @platform, @image, @featured)";

        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
        MySqlDataReader myReader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
            {
                myCommand.Parameters.AddWithValue("@idUser", reviews.IdUser);
                myCommand.Parameters.AddWithValue("@review", reviews.Review);
                myCommand.Parameters.AddWithValue("@title", reviews.Title);
                myCommand.Parameters.AddWithValue("@rating", reviews.Rating);
                myCommand.Parameters.AddWithValue("@ratingReasoning", reviews.RatingReasoning);
                myCommand.Parameters.AddWithValue("@platform", reviews.Platform);
                myCommand.Parameters.AddWithValue("@image", reviews.Image);
                myCommand.Parameters.AddWithValue("@featured", reviews.Featured);
                
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                mycon.Close();
            }
        }
        return new JsonResult("Added Successfully");
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public JsonResult Put(Reviews reviews, int id)
    {
        var query = @"
                        update reviews set 
                        idUser =@idUser, review=@review, title=@title, rating=@rating, ratingReasoning=@ratingReasoning, platform=@platform, image=@image, featured=@featured
                        where id=@id;
            ";

        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
        MySqlDataReader myReader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
            {
                myCommand.Parameters.AddWithValue("@id", id);
                myCommand.Parameters.AddWithValue("@idUser", reviews.IdUser);
                myCommand.Parameters.AddWithValue("@review", reviews.Review);
                myCommand.Parameters.AddWithValue("@title", reviews.Title);
                myCommand.Parameters.AddWithValue("@rating", reviews.Rating);
                myCommand.Parameters.AddWithValue("@ratingReasoning", reviews.RatingReasoning);
                myCommand.Parameters.AddWithValue("@platform", reviews.Platform);
                myCommand.Parameters.AddWithValue("@image", reviews.Image);
                myCommand.Parameters.AddWithValue("@featured", reviews.Featured);
                
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                mycon.Close();
            }
        }
        return new JsonResult("Updated Successfully");
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public JsonResult Delete(int id)
    {
        var query = @"
                        delete from reviews
                        where id=@id;
                        
            ";

        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
        MySqlDataReader myReader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
            {
                myCommand.Parameters.AddWithValue("@id", id);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                mycon.Close();
            }
        }

        return new JsonResult("Deleted Successfully");
    }
}