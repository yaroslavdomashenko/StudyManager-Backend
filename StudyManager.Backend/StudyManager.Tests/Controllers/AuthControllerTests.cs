using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyManager.Controllers;
using StudyManager.Data.Entities;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyManager.Tests.Controllers
{
    public class AuthControllerTests
    {
        public readonly Mock<IAuthService> _authService = new Mock<IAuthService>();


        [Fact]
        public async Task Register_LoginIsBusy_ReturnsBadRequest()
        {
            // Arrange
            var regModel = CreateRegisterModel();
            _authService.Setup(x => x.Register(regModel))
                .ReturnsAsync(false);
            var controller = new AuthController(_authService.Object);

            // Act
            var actionResult = await controller.Register(regModel);

            // Assert
            var BadRequestResult = actionResult as BadRequestObjectResult;
            BadRequestResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_LoginIsFree_ReturnsOk()
        {
            // Arrange
            var regModel = CreateRegisterModel();
            _authService.Setup(x => x.Register(regModel))
                .ReturnsAsync(true);
            var controller = new AuthController(_authService.Object);

            // Act 
            var actionResult = await controller.Register(regModel);

            // Assert
            var okResult = actionResult as OkResult;
            okResult.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Login_WrongInputs_ReturnBadRequest()
        {
            // Arrange
            var loginModel = CreateLoginModel();
            _authService.Setup(x => x.Login(loginModel))
                .ReturnsAsync((string)null);
            var controller = new AuthController(_authService.Object);

            // Act
            var actionResult = await controller.Login(loginModel);

            // Assert
            var BadRequestResult = actionResult as BadRequestObjectResult;
            BadRequestResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_CorrectInputs_ReturnToken()
        {
            // Arrange
            var loginModel = CreateLoginModel();
            _authService.Setup(x => x.Login(loginModel))
                .ReturnsAsync(Guid.NewGuid().ToString());
            var controller = new AuthController(_authService.Object);

            // Act
            var actionResult = await controller.Login(loginModel);

            // Assert
            var okResult = actionResult as OkObjectResult;
            okResult.Value.Should().NotBeNull();
        }

        private LoginModel CreateLoginModel()
        {
            return new LoginModel
            {
                Login = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
        }
        private RegisterModel CreateRegisterModel()
        {
            return new RegisterModel
            {
                Login = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
                Surename = Guid.NewGuid().ToString()
            };
        }
    }
}
