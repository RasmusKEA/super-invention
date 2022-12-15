namespace Test_exam.Models;

public class Comment
{
    public int Id { get; set; }
    public int IdUser {get; set;}
    public int idReview { get; set; }
    public string UserComment { get; set; }
}