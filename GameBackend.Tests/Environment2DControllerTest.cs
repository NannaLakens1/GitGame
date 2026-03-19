using Microsoft.AspNetCore.Mvc;
using Moq;
using GameBackend.Controllers;
using GameBackend.Services;
using GameBackend.Repositories;
using GameBackend.Models;

namespace GameBackend.Tests
{
    [TestClass]
    public sealed class Environment2DTests
    {
        private PatientController environment2DController;
        private Mock<IPatientRepository> environment2DRepository;
        private Mock<IAuthenticationService> authenticationService;
        //private Object2DController Object2DController;
        //private Mock<IObject2DRepository> object2DRepository;

        [TestInitialize]
        public void Setup()
        {
            environment2DRepository = new Mock<IPatientRepository>();
            authenticationService = new Mock<IAuthenticationService>();

            environment2DController = new PatientController(environment2DRepository.Object, authenticationService.Object);
        }

        [TestMethod]
        public async Task AddEnvironment_NameEmpty_ReturnsBadRequest()
        {
            // arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("5trdxcvhHGyt7gGvjbbn");

            var env = new Patient
            {
                Name = ""
            };

            // act
            var result = await environment2DController.AddAsync(env);

            // result
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            environment2DRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Never);
        }

        [TestMethod]
        public async Task AddEnvironment_ValidInput_UserLoggedIn_WorldCreated()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("Vhgfhb&%RTYHvbhu77*&TTYH");

            var env = new Patient
            {
                Name = "NewWorld"
            };

            // Act
            var result = await environment2DController.AddAsync(env);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            environment2DRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Once);
        }
        [TestMethod]
        public async Task AddEnvironment_UserNotLoggedIn_ReturnsBadRequest()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns((string?)null);

            var env = new Patient
            {
                Name = "NewWorld"
            };

            // Act
            var result = await environment2DController.AddAsync(env);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            environment2DRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Never);
        }
        public async Task AddEnvironment_Already5Environments_ReturnsBadRequest()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("vfrt5tGFghg7tyhgh");

            environment2DRepository.Setup(r => r.SelectAsyncByUserId("vfrt5tGFghg7tyhgh")).ReturnsAsync(new List<Patient>
            {
                new Patient(),
                new Patient(),
                new Patient(),
                new Patient(),
                new Patient()
            });

            var env = new Patient
            {
                Name = "NewWorld"
            };

            // Act
            var result = await environment2DController.AddAsync(env);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            environment2DRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Never);
        }

        [TestMethod]
        public async Task AddEnvironment_DuplicateName_ReturnsBadRequest()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("vfrt5tGFghg7tyhgh");

            environment2DRepository.Setup(r => r.SelectAsyncByUserId("vfrt5tGFghg7tyhgh")).ReturnsAsync(new List<Patient>
            {
                new Patient { Name = "ExistingWorld" }
            });

            var env = new Patient
            {
                Name = "ExistingWorld"
            };

            // Act
            var result = await environment2DController.AddAsync(env);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            environment2DRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Never);
        }
    }
}