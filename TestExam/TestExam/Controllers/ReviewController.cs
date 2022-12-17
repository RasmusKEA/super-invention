using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Test_exam.Models;
using Test_exam.Services;

namespace Test_exam.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IReviewService _reviewService;

    public ReviewController(IConfiguration configuration, IReviewService reviewService)
    {
        _configuration = configuration;
        _reviewService = reviewService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return _reviewService.GetAllReviews();
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        return _reviewService.GetById(id);
    }
    
    [HttpGet("featured")]
    public IActionResult GetFeatured()
    {
        return _reviewService.GetFeatured();
    }
    [Authorize]
    [HttpPost]
    public IActionResult Post(Reviews reviews)
    {
        return _reviewService.PostReview(reviews);
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public IActionResult Put(Reviews reviews, int id)
    {
        return _reviewService.PutReview(reviews, id);
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return _reviewService.DeleteReview(id);
    }
}