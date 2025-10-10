using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using PolicyHolderFunction;

namespace TestPolicyHolderFunction
{
    public class PolicyHolderFunctionTests
    {
        [Fact]
        public void Run_ShouldReturn_OkObjectResult_With_WelcomeMessage()
        {
            // Arrange
            var logger = new NullLogger<Function1>();
            var function = new Function1(logger);

            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Body = new MemoryStream(); // empty body

            // Act
            var result = function.Run(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Welcome to Azure Functions!", okResult.Value);
        }
    }
}
