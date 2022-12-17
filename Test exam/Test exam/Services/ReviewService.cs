using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Test_exam.Models;

namespace Test_exam.Services;

public interface IReviewService
{
    IActionResult GetAllReviews();
    IActionResult GetById(int id);
    IActionResult GetFeatured();
    IActionResult PostReview(Reviews reviews);
    IActionResult PutReview(Reviews reviews, int id);
    IActionResult DeleteReview(int id);
}

public class ReviewService : IReviewService
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString = "Server=localhost;Database=rategame;Username=user;Password=password";

    public ReviewService()
    {
    }

    public ReviewService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult GetAllReviews()
    {
        const string query = @"SELECT * FROM reviews ORDER BY createdAt ASC";
        var table = new DataTable();
        var sqlDataSource = _connectionString;//_configuration.GetConnectionString("MySQLCon");
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

        if (table.Rows.Count == 0)
        {
            return new NotFoundResult();
        }
        
        return new JsonResult(table);
    }

    public IActionResult GetById(int id)
    {
        var query = @"SELECT * FROM reviews WHERE id = @id";
        var table = new DataTable();
        var sqlDataSource = _connectionString;
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

        if (table.Rows.Count == 0)
        {
            return new NotFoundResult();
        }

        var review = new Reviews()
        {
            Id = table.Rows[0].Field<int>("id"),
            IdUser = table.Rows[0].Field<int>("idUser"),
            Review = table.Rows[0].Field<string>("review"),
            Title = table.Rows[0].Field<string>("title"),
            Rating = table.Rows[0].Field<int>("rating"),
            RatingReasoning = table.Rows[0].Field<string>("ratingReasoning"),
            Platform = table.Rows[0].Field<string>("platform"),
            Image = table.Rows[0].Field<string>("image"),
        };

        return new OkObjectResult(review);
    }

    public IActionResult GetFeatured()
    {
        var query = @"SELECT * FROM reviews WHERE featured = 1";
        var table = new DataTable();
        var sqlDataSource = _connectionString;
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

        if (table.Rows.Count == 0)
        {
            return new NotFoundResult();
        }

        return new JsonResult(table);
    }

    public IActionResult PostReview(Reviews reviews)
    {
        var query = @"INSERT INTO reviews (idUser, review, title, rating, ratingReasoning, platform, image, featured) 
        values (@idUser, @review, @title, @rating, @ratingReasoning, @platform, @image, @featured)";

        var table = new DataTable();
        var sqlDataSource = _connectionString;
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

        return new OkResult();
    }

    public IActionResult PutReview(Reviews reviews, int id)
    {
        var query = @"
                        update reviews set 
                        idUser =@idUser, review=@review, title=@title, rating=@rating, ratingReasoning=@ratingReasoning, platform=@platform, image=@image, featured=@featured
                        where id=@id;
            ";

        var table = new DataTable();
        var sqlDataSource = _connectionString;
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

        return new OkResult();
    }

    public IActionResult DeleteReview(int id)
    {
        var query = @"delete from reviews
                      where id=@id;";

        var table = new DataTable();
        var sqlDataSource = _connectionString;
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

        return new OkResult();
    }
}