using NUnit.Framework;
using FluentAssertions;
using Test_exam.Models;
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
    public void VerifyFeaturedReviewExists()
    {
        //Arrange 
        var reviewService = new ReviewService();

        //Act
        var actual = reviewService.GetFeatured();
        
        //Assert
        Assert.AreEqual(200, actual.GetType().GetProperty("StatusCode").GetValue(actual, null));
    }

    [Test]
    public void VerifyDeletionOfReview()
    {
        //Arrange 
        var reviewService = new ReviewService();
        var newReview = new Reviews
        {
            IdUser = 11,
            Review = "This is a test review for deletion",
            Title = "This is a test title for deletion",
            Rating = 10,
            RatingReasoning = "This is a test rating reasoning for deletion",
            Platform = "Platform for deletion",
            Image = "Image",
            Featured = 0
        };
        var postReview = reviewService.PostReview(newReview);
        
        //Act
        var latestReview = reviewService.GetLatestReview();
        var deleteReview = reviewService.DeleteReview(latestReview.Id);
        
        //Assert
        Assert.AreEqual(200,postReview.GetType().GetProperty("StatusCode").GetValue(postReview, null));
        Assert.IsNotNull(latestReview);
        Assert.AreEqual(200,deleteReview.GetType().GetProperty("StatusCode").GetValue(deleteReview, null));
    }

    [Test]
    public void VerifyPutReview()
    {
        //Arrange
        var reviewService = new ReviewService();
        var newReview = new Reviews
        {
            IdUser = 11,
            Review = "This is a test review for deletion",
            Title = "This is a test title for deletion",
            Rating = 10,
            RatingReasoning = "This is a test rating reasoning for deletion",
            Platform = "Platform for deletion",
            Image = "Image",
            Featured = 0
        };
        var postReview = reviewService.PostReview(newReview);
        //Act
        var latestReview = reviewService.GetLatestReview();
        var putReview = reviewService.PutReview(newReview, latestReview.Id);
        var deleteReview = reviewService.DeleteReview(latestReview.Id);
        
        //Assert
        Assert.AreEqual(200,postReview.GetType().GetProperty("StatusCode").GetValue(postReview, null));
        Assert.IsNotNull(latestReview);
        Assert.AreEqual(200,putReview.GetType().GetProperty("StatusCode").GetValue(putReview, null));
        Assert.AreEqual(200,deleteReview.GetType().GetProperty("StatusCode").GetValue(deleteReview, null));
    }

}