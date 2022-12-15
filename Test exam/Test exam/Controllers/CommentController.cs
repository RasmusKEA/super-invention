using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Test_exam.Models;

namespace Test_exam.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class CommentController
{
    private readonly IConfiguration _configuration;

    public CommentController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("{idReview:int}")]
    public JsonResult GetById(int idReview)
    {
        var query = @"SELECT * FROM comments WHERE idReview = @idReview";
        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
        MySqlDataReader reader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand command = new MySqlCommand(query, mycon))
            {
                command.Parameters.AddWithValue("@idReview", idReview);
                reader = command.ExecuteReader();
                table.Load(reader);
                mycon.Close();
            }
        }
        return new JsonResult(table);
    }

    [HttpPost]
    public JsonResult Post(Comment comment)
    {
        var query = @"INSERT INTO comments (idUser, idReview, userComment) 
        values (@idUser, @idReview, @userComment)";

        var table = new DataTable();
        var sqlDataSource = _configuration.GetConnectionString("MySQLCon");
        MySqlDataReader myReader;
        using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        {
            mycon.Open();
            using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
            {
                myCommand.Parameters.AddWithValue("@idUser", comment.IdUser);
                myCommand.Parameters.AddWithValue("@idReview", comment.idReview);
                myCommand.Parameters.AddWithValue("@userComment", comment.UserComment);
                
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                mycon.Close();
            }
        }
        return new JsonResult("Added Successfully");
    }

    [HttpPut("{id}")]
    public JsonResult Put(Comment comment, int id)
    {
        var query = @"
                        update comments set 
                        idUser =@idUser, idReview=@idReview, userComment=@userComment
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
                myCommand.Parameters.AddWithValue("@idUser", comment.IdUser);
                myCommand.Parameters.AddWithValue("@idReview", comment.idReview);
                myCommand.Parameters.AddWithValue("@userComment", comment.UserComment);
                
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                mycon.Close();
            }
        }
        return new JsonResult("Updated Successfully");
    }
    
    [HttpDelete("{id}")]
    public JsonResult Delete(int id)
    {
        var query = @"
                        delete from comments
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