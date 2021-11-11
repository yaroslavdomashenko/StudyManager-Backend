using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyManager.Controllers;
using StudyManager.Data.Entities;
using StudyManager.Data.Infrastructure;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyManager.Tests.Controllers
{
    public class AdminControllerTests
    {
        private readonly Mock<IAdminService> _adminService = new Mock<IAdminService>();

        [Fact]
        public async Task ChangeRoleId_WrongInputs_ReturnsBadRequest()
        {
            // Arrange
            _adminService.Setup(x => x.ChangeRole("TestName", Guid.NewGuid(), Role.Student))
                .ReturnsAsync(false);
            var controller = CreateControllerWithUserIdentity();

            // Act
            var actionResult = await controller.ChangeRoleId(Guid.NewGuid(), Role.Student);

            // Assert
            var badRequest = actionResult as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task ChangeRoleLogin_WrongInputs_ReturnsBadRequest()
        {
            // Arrange 
            _adminService.Setup(x => x.ChangeRole("Test", "Test", Role.Admin))
                .ReturnsAsync(false);
            var controller = CreateControllerWithUserIdentity();

            // Act
            var actionResult = await controller.ChangeRoleLogin("Test", Role.Student);

            // Assert
            actionResult.Should().BeOfType<BadRequestObjectResult>();
            (actionResult as BadRequestObjectResult).Should().NotBeNull();
        }

        private AdminController CreateControllerWithUserIdentity()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "MockName"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            var controller = new AdminController(_adminService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;
        }

    }
}
