using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyManager.Controllers;
using StudyManager.Data.Models;
using StudyManager.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace StudyManager.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> userService = new Mock<IUserService>();
        private readonly Mock<IFileService> fileService = new Mock<IFileService>();

        
        [Fact]
        public async Task GetUser_ByGuid_WithUnexistingUser_ReturnsNotFound()
        {
            // Arrange
            userService.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync((UserModel)null);
            var controller = new UsersController(userService.Object, fileService.Object);

            // Act
            var actionResult = await controller.GetUser(Guid.NewGuid());

            // Assert
            var NotFoundObjectResult = actionResult as NotFoundObjectResult;
            NotFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetUser_ByGuid_WithExistingUser_ReturnsUserModel()
        {
            // Arrange
            var expectedUser = CreateRandomUserModel();
            userService.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(expectedUser);

            var controller = new UsersController(userService.Object, fileService.Object);

            // Act
            var result = await controller.GetUser(Guid.NewGuid());

            // Assert
            var OkObjectResult = result as OkObjectResult;
            OkObjectResult.Should().NotBeNull();

            var model = OkObjectResult.Value as UserModel;
            model.Should().NotBeNull();
            model.Should().BeEquivalentTo(expectedUser, opt => opt.ComparingByMembers<UserModel>());
        }

        [Fact]
        public async Task GetUser_ByLogin_WithUnexistingUser_ReturnsNotFound()
        {
            // Arrange
            userService.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync((UserModel)null);
            var controller = new UsersController(userService.Object, fileService.Object);

            // Act
            var actionResult = await controller.GetUser(Guid.NewGuid().ToString());

            // Assert
            var NotFoundObjectResult = actionResult as NotFoundObjectResult;
            NotFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetUser_ByLogin_WithExistingUser_ReturnsUserModel()
        {
            // Arrange
            var expectedUser = CreateRandomUserModel();
            userService.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            var controller = new UsersController(userService.Object, fileService.Object);

            // Act
            var result = await controller.GetUser(Guid.NewGuid().ToString());

            // Assert
            var OkObjectResult = result as OkObjectResult;
            OkObjectResult.Should().NotBeNull();

            var model = OkObjectResult.Value as UserModel;
            model.Should().NotBeNull();
            model.Should().BeEquivalentTo(expectedUser, opt => opt.ComparingByMembers<UserModel>());
        }

        [Fact]
        public async Task GetMe_WithUnexistingUser_ReturnsNotFound()
        {
            // Arrange
            userService.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync((UserModel)null);

            var controller = CreateControllerWithUserIdentity();

            // Act
            var actionResult = await controller.GetMe();

            // Assert
            var NotFoundObjectResult = actionResult as NotFoundObjectResult;
            NotFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetMe_WithExistingUser_ReturnsUserModel()
        {
            // Arrange
            var expectedUser = CreateRandomUserModel();
            userService.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            var controller = CreateControllerWithUserIdentity();

            // Act
            var result = await controller.GetMe();

            // Assert
            var OkObjectResult = result as OkObjectResult;
            OkObjectResult.Should().NotBeNull();

            var model = OkObjectResult.Value as UserModel;
            model.Should().NotBeNull();
            model.Should().BeEquivalentTo(expectedUser, opt => opt.ComparingByMembers<UserModel>());
        }



        private UsersController CreateControllerWithUserIdentity()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "MockName"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            var controller = new UsersController(userService.Object, fileService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;
        }
        private UserModel CreateRandomUserModel()
        {
            return new UserModel
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Surename = Guid.NewGuid().ToString(),
                Login = Guid.NewGuid().ToString(),
                DateCreated = DateTime.UtcNow,
                Avatar = Guid.NewGuid().ToString()
            };
        }
    }
}
