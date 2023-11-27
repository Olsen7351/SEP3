using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Broker_Test.Controller_Test;

public class User_ControllerTest
{
    [Fact]
        public async void CreateUser_ReturnsOk_WhenUserIsNotNull()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);
            var user = new User(); // Replace with valid user data

            // Mock the UserService to return an OkObjectResult for a successful user creation
            mockUserService.Setup(service => service.CreateUser(user))
                .ReturnsAsync(new OkObjectResult("User created successfully"));

            // Act
            var result = await controller.CreateUser(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created successfully", okResult.Value);
        }

        [Fact]
        public async void CreateUser_ReturnsBadRequest_WhenUserIsNull()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);
            User nullUser = null; // User is null

            // Act
            var result = await controller.CreateUser(nullUser);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void LoginWithUserCredentials_ReturnsOk_WhenUserIsNotNull()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);
            var user = new User(); // Replace with valid user data

            // Mock the UserService to return an OkObjectResult for a successful login
            mockUserService.Setup(service => service.LoginWithUserCredentials(user))
                .ReturnsAsync(new OkObjectResult("Login successful"));

            // Act
            var result = await controller.LoginWithUserCredentials(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Login successful", okResult.Value);
        }

        [Fact]
        public async void LoginWithUserCredentials_ReturnsBadRequest_WhenUserIsNull()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);
            User nullUser = null; // User is null

            // Act
            var result = await controller.LoginWithUserCredentials(nullUser);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }

