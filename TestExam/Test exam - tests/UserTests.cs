using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Test_exam.Services;

namespace Test_exam___tests;

public class UserTests
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    

    public UserTests(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [Test]
    public void VerifyUsernameTest()
    {
        //Arrange 
        var userService = new UserService();
        
        //Act
        var actual = userService.VerifyUsername("admin");
        var expected = false;

        //Assert
        Assert.AreNotEqual(actual, expected);
    }
}