using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyManager.Controllers;
using StudyManager.Data.Models.Chat;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudyManager.Tests.Controllers
{
    public class ChatControllerTests
    {
        private readonly Mock<IChatService> _chatService = new Mock<IChatService>();

        [Fact]
        public async Task GetMessages_ReturnsOk()
        {
            // Arrange
            var messages = GetListMessages();
            _chatService.Setup(x => x.GetMessages(Guid.NewGuid(), 0))
                .ReturnsAsync(messages);
            var controller = new ChatController(_chatService.Object);

            // Act
            var actionResult = await controller.GetMessages(Guid.NewGuid());

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetMessages_ReturnsNothing()
        {
            // Arrange
            var messages = GetListMessages();
            _chatService.Setup(x => x.GetMessages(Guid.NewGuid(), 0))
                .ReturnsAsync((List<MessageDto>)null);
            var controller = new ChatController(_chatService.Object);

            // Act
            var actionResult = await controller.GetMessages(Guid.NewGuid());

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        private List<MessageDto> GetListMessages()
        {
            return new List<MessageDto>
            {
                new MessageDto { Id = Guid.NewGuid(), DateCreated = DateTime.Now, Text = "Test" },
                new MessageDto { Id = Guid.NewGuid(), DateCreated = DateTime.Now, Text = "Test" },
                new MessageDto { Id = Guid.NewGuid(), DateCreated = DateTime.Now, Text = "Test" },
                new MessageDto { Id = Guid.NewGuid(), DateCreated = DateTime.Now, Text = "Test" }
            };
        }
    }
}
