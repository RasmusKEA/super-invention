using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Test_exam.Services;

namespace Test_exam.UnitTests;

public class ReviewTest
{
    [Test]
    public void VerifyRetrievalOfReviews()
    {
        //Arrange 
        var reviewService = new ReviewService();

        //Act
        var actual = reviewService.GetAllReviews();
        
        //Assert
        Assert.IsNotNull(actual);
    }
    
    [Test]
    public void VerifyRetrievalOfAReviews()
    {
        //Arrange 
        var reviewService = new ReviewService();

        //Act
        var actual = reviewService.GetById(7);
        Console.WriteLine(actual);
        
        //Assert
        Assert.IsNotNull(actual);
    }
    
    [Test]
    public void VerifyRetrievalOfNotExistingReview()
    {
        //Arrange 
        var reviewService = new ReviewService();

        //Act
        var actual = reviewService.GetById(700);
        
        //Assert
        Assert.AreEqual(404, actual.GetType().GetProperty("StatusCode").GetValue(actual, null));
    }

    [Test]
    public void VerifyDeletionOfReview()
    {
        //Arrange 
        var reviewService = new ReviewService();

        //Act
        var actual = reviewService.GetAllReviews();
        Console.WriteLine(JsonConvert.SerializeObject( actual, Formatting.Indented));
        //Assert

    }

}