/*using Microsoft.AspNetCore.Mvc;
using Moq;
using GameBackend.Controllers;
using GameBackend.Services;
using GameBackend.Repositories;
using GameBackend.Models;

namespace GameBackend.Tests
{
    [TestClass]
    public sealed class Environment2DControllerTest
    {
        private Environment2DController controller;
        private Mock<IEnvironment2DRepository> environment2dRepository;

        [TestInitialize]
        public void Setup()
        {
            environment2dRepository = new Mock<IEnvironment2DRepository>();
            controller = new Environment2DController(environment2dRepository.Object);
        }

        [TestMethod]
        public async Task Get_ExampleObjectThatDoesNotExist_Returns404NotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            environment2dRepository.Setup(x => x.SelectAsync(id)).ReturnsAsync(null as Environment2D);

            // Act
            var response = await controller.GetByIdAsync(id);

            // Assert
            Assert.IsInstanceOfType<NotFoundObjectResult>(response.Result);
        }
    }
}
*/
