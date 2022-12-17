using NUnit.Framework;
using Test_exam.Models;
using Test_exam.Services;
using FluentAssertions;

namespace Test_exam.UnitTests;

public class UserTests
{
    
    [Test]
    public void VerifyUsernameUnavailable()
    {
        //Arrange 
        var userService = new UserService();

        //Act
        var actual = userService.VerifyUsername("admin");
        const bool expected = false;
        
        //Assert
        Assert.AreEqual(actual, expected);
    }
    
    [Test]
    public void VerifyUsernameAvailable()
    {
        //Arrange 
        var userService = new UserService();

        //Act
        var actual = userService.VerifyUsername("thisisanavailableusername");
        const bool expected = true;
        
        //Assert
        Assert.AreEqual(actual, expected);
    }
    
    [Test]
    public void VerifyEmailUnavailable()
    {
        //Arrange 
        var userService = new UserService();

        //Act
        var actual = userService.VerifyEmail("admin@admin.dk");
        const bool expected = false;
        
        //Assert
        Assert.AreEqual(actual, expected);
    }
    
    [Test]
    public void VerifyEmailAvailable()
    {
        //Arrange 
        var userService = new UserService();

        //Act
        var actual = userService.VerifyEmail("available@mail.dk");
        const bool expected = true;
        
        //Assert
        Assert.AreEqual(actual, expected);
    }

    [Test]
    public void VerifyLoginCorrect()
    {
        //Arrange 
        var userService = new UserService();
        var user = new User
        {
            Username = "admin",
            Password = "admin"
        };

        //Act
        var actual = userService.VerifyLogin(user);
        var expected = new User
        {
            Id = 11,
            Username = "admin",
            Email = "admin@admin.dk"
        };

        //Assert
        expected.Should().BeEquivalentTo(actual);
    }
    
    [Test]
    public void VerifyLoginIncorrect()
    {
        //Arrange 
        var userService = new UserService();
        var user = new User
        {
            Username = "admin",
            Password = "admin"
        };

        //Act
        var actual = userService.VerifyLogin(user);
        var expected = new User
        {
            Id = 111,
            Username = "notAdmin",
            Email = "not@admin.dk"
        };

        //Assert
        expected.Should().NotBeEquivalentTo(actual);
    }

    [Test]
    public void VerifyTokenAndRoleIsSet()
    {
        var userService = new UserService();
        var user = new User
        {
            Id = 11,
            Username = "admin",
            Email = "admin@admin.dk",
            Password = "$2a$08$StgSM4VP7SVdzs10B7K39uCOXweg6.AnGC2YAxIvSP8PO5vEApDCy"
        };

        //Act
        var actual = userService.SetRoleAndToken(user);

        Assert.IsNotNull(actual.Token);
        Assert.IsNotNull(actual.Role);
    }


}