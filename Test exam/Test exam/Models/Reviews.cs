namespace Test_exam.Models;

public class Reviews
{
    public int Id { get; set; }

    public int IdUser {get; set;}
    public string Review {get; set;}
    public string Title { get; set; }
    public int Rating { get; set; }
    public string RatingReasoning { get; set; }
    public string Platform { get; set; }
    public string Image { get; set; }
    public int Featured { get; set; }
}