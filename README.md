Это мой контроллер с 3 ендпоинтами, как лучше написатть юнит тесты для них? 

namespace SPM3._0Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrancheController : ControllerBase
    {
        private readonly ITrancheService _trancheService;
        private readonly ILogger<TrancheController> _logger;

        public TrancheController(ITrancheService trancheService, ILogger<TrancheController> logger)
        {
            _trancheService = trancheService;
            _logger = logger;
        }

        [Route("GetAuthorities/{branchNumber}")]
        [HttpGet]
        public async Task<IActionResult> GetAuthorities(string branchNumber)
        {
            _logger.LogInformation($"GetAuthorities || branchNumber: {branchNumber}");
            if (string.IsNullOrEmpty(branchNumber))
            {
                return BadRequest("Null parametr: branchNumber");
            }
            var result = await _trancheService.GetAuthoritiesAsync(branchNumber);
            if (result.Count() == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [Route("GetResultLoan/{refer}")]
        [HttpGet]
        public async Task<IActionResult> GetResultLoan(string refer)
        {
            _logger.LogInformation($"GetResultLoan || refer: {refer}");
            if (string.IsNullOrEmpty(refer))
            {
                return BadRequest("Null parametr: refer");
            }
            var result = await _trancheService.GetResultLoanAsync(refer);
            return Ok(result);
        }


        [Route("GetDocumentsFromHDD/{creditLineNumber}")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentsFromHDD(string creditLineNumber)
        {
            _logger.LogInformation($"GetDocumentsFromHDD || creditLineNumber: {creditLineNumber}");
            if (string.IsNullOrEmpty(creditLineNumber))
            {
                return BadRequest("Null parametr: creditLineNumber");
            }
            var result = await _trancheService.GetDocumentsFromHDDAsync(creditLineNumber);
            if (result.Count() == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
Это мои юниттесты 
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SPM3._0Service.Controllers;
using SPM3._0Service.Services;
using SPM3._0Service3._0.UnitTests.MockData;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SPM3._0Service3._0.UnitTests.Controllers
{
    public class TestTrancheController
    {
        [Fact]
        public async Task GetAuthoritiesAsync_ShouldReturn200Status()
        {
            /// Arrange
            string testBranchNumber = "DRKK1";
            var todoServiceMock = new Mock<ITrancheService>();
            var loggerMock = new Mock<ILogger<TrancheController>>();
            todoServiceMock.Setup(service => service.GetAuthoritiesAsync(testBranchNumber)).ReturnsAsync(TrancheMockData.GetAuthoritiesData());
            var _controller = new TrancheController(todoServiceMock.Object, loggerMock.Object);

            /// Act
            var result = (OkObjectResult)await _controller.GetAuthorities(testBranchNumber);

            // /// Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAuthoritiesAsync_ShouldReturn204NoContentStatus()
        {
            /// Arrange
            string testBranchNumber = "Random";
            var todoServiceMock = new Mock<ITrancheService>();
            var loggerMock = new Mock<ILogger<TrancheController>>();
            todoServiceMock.Setup(service => service.GetAuthoritiesAsync(testBranchNumber)).ReturnsAsync(TrancheMockData.GetAuthoritiesDataEmpty());
            var _controller = new TrancheController(todoServiceMock.Object, loggerMock.Object);

            /// Act
            var result = (NotFoundResult)await _controller.GetAuthorities(testBranchNumber);

            /// Assert
            result.StatusCode.Should().Be(404);
            todoServiceMock.Verify(service => service.GetAuthoritiesAsync(testBranchNumber), Times.Exactly(1));
        }

        [Fact]
        public async Task GetAuthoritiesAsync_ShouldReturn400BadRequestStatus()
        {
            /// Arrange
            var todoServiceMock = new Mock<ITrancheService>();
            var loggerMock = new Mock<ILogger<TrancheController>>();
            todoServiceMock.Setup(service => service.GetAuthoritiesAsync(""));
            var _controller = new TrancheController(todoServiceMock.Object, loggerMock.Object);

            /// Act
            var result = (BadRequestObjectResult)await _controller.GetAuthorities("");

            // /// Assert
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetResultLoanAsync_ShouldReturn200Status()
        {
            /// Arrange
            string refer = "Success";
            var todoServiceMock = new Mock<ITrancheService>();
            var loggerMock = new Mock<ILogger<TrancheController>>();
            todoServiceMock.Setup(service => service.GetResultLoanAsync(refer)).ReturnsAsync(TrancheMockData.GetLoanResultData(refer));
            var _controller = new TrancheController(todoServiceMock.Object, loggerMock.Object);

            /// Act
            var result = (OkObjectResult)await _controller.GetResultLoan(refer);

            // /// Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetResultLoanAsync_ShouldReturn400BadRequestStatus()
        {
            /// Arrange
            var todoServiceMock = new Mock<ITrancheService>();
            var loggerMock = new Mock<ILogger<TrancheController>>();
            todoServiceMock.Setup(service => service.GetResultLoanAsync(""));
            var _controller = new TrancheController(todoServiceMock.Object, loggerMock.Object);

            /// Act
            var result = (BadRequestObjectResult)await _controller.GetResultLoan("");

            // /// Assert
            result.StatusCode.Should().Be(400);
        }


        [Fact]
        public async Task GetDocumentsFromHDDAsync_ShouldReturn200Status()
        {
            /// Arrange
            string branchNumber = "47510042465";
            var todoServiceMock = new Mock<ITrancheService>();
            var loggerMock = new Mock<ILogger<TrancheController>>();
            todoServiceMock.Setup(service => service.GetDocumentsFromHDDAsync(branchNumber)).ReturnsAsync(TrancheMockData.GetDocumentsData());
            var _controller = new TrancheController(todoServiceMock.Object, loggerMock.Object);

            /// Act
            var result = (OkObjectResult)await _controller.GetDocumentsFromHDD(branchNumber);

            // /// Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetDocumentsFromHDDAsync_ShouldReturn204NoContentStatus()
        {
            /// Arrange
            string branchNumber = "Empty";
            var todoServiceMock = new Mock<ITrancheService>();
            var loggerMock = new Mock<ILogger<TrancheController>>();
            todoServiceMock.Setup(service => service.GetDocumentsFromHDDAsync(branchNumber)).ReturnsAsync(TrancheMockData.GetDocumentsDataEmpty());
            var _controller = new TrancheController(todoServiceMock.Object, loggerMock.Object);

            /// Act
            var result = (NotFoundResult)await _controller.GetDocumentsFromHDD(branchNumber);

            /// Assert
            result.StatusCode.Should().Be(404);
            todoServiceMock.Verify(service => service.GetDocumentsFromHDDAsync(branchNumber), Times.Exactly(1));
        }

        [Fact]
        public async Task GetDocumentsFromHDDAsync_ShouldReturn400BadRequestStatus()
        {
            /// Arrange
            var todoServiceMock = new Mock<ITrancheService>();
            var loggerMock = new Mock<ILogger<TrancheController>>();
            todoServiceMock.Setup(service => service.GetDocumentsFromHDDAsync(""));
            var _controller = new TrancheController(todoServiceMock.Object, loggerMock.Object);

            /// Act
            var result = (BadRequestObjectResult)await _controller.GetDocumentsFromHDD("");

            // /// Assert
            result.StatusCode.Should().Be(400);
        }
    }
}
