using Moq;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudyManager.Tests.Controllers
{
    public class CommentsControllerTests
    {
        private readonly Mock<ICommentsService> _commentsService = new Mock<ICommentsService>();

        [Fact]
        public async Task CreateComment_CorrectInputs_ReturnOk()
        {

        }
    }
}
