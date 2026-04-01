using Microsoft.AspNetCore.Mvc;
using Moq;
using GameBackend.Controllers;
using GameBackend.Services;
using GameBackend.Repositories;
using GameBackend.Models;

namespace GameBackend.Tests
{
    
    [TestClass]
    public sealed class ControllerTest
    {
        private PatientController patientController;
        private Mock<IPatientRepository> patientRepository;
        private Mock<IAuthenticationService> authenticationService;
        private BehandelingController behandelingController;
        private Mock<IBehandelingRepository> behandelingRepository;

        [TestInitialize]
        public void Setup()
        {
            patientRepository = new Mock<IPatientRepository>();
            authenticationService = new Mock<IAuthenticationService>();

            patientController = new PatientController(patientRepository.Object, authenticationService.Object);
            behandelingRepository = new Mock<IBehandelingRepository>();
            behandelingController = new BehandelingController(behandelingRepository.Object, authenticationService.Object, patientRepository.Object);
        }

        [TestMethod]
        public async Task AddPatient_NameEmpty_ReturnsBadRequest()
        {
            // arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("5trdxcvhHGyt7gGvjbbn");

            var env = new Patient
            {
                Name = "",
                Age = 11
            };

            // act
            var result = await patientController.AddAsync(env);

            // result
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            patientRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Never);
        }

        [TestMethod]
        public async Task AddPatient_ValidInput_UserLoggedIn_PatientCreated()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("Vhgfhb&%RTYHvbhu77*&TTYH");

            var env = new Patient
            {
                Name = "Bella",
                Age = 11
            };

            // Act
            var result = await patientController.AddAsync(env);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            patientRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Once);
        }
        [TestMethod]
        public async Task AddPatient_UserNotLoggedIn_ReturnsBadRequest()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns((string?)null);

            var env = new Patient
            {
                Name = "Bella",
                Age = 11
            };

            // Act
            var result = await patientController.AddAsync(env);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

            patientRepository.Verify(r => r.InsertAsync(It.IsAny<Patient>()), Times.Never);
        }

        [TestMethod]
        public async Task GetBehandelingByPatient_NotLoggedIn_ReturnsUnauthorized()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns((string?)null);

            // Act
            var result = await behandelingController.GetByPatient(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            behandelingRepository.Verify(r => r.SelectByPatientAsync(It.IsAny<Guid>()), Times.Never);
            patientRepository.Verify(r => r.SelectAsync(It.IsAny<Guid>()),Times.Never);
        }

        [TestMethod]
        public async Task AddBehandeling_WithoutArts_ReturnsCreated()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("Vhgfhb&%RTYHvbhu77*&TTYH");

            var patientId = Guid.NewGuid();
            patientRepository.Setup(x => x.SelectAsync(patientId)).ReturnsAsync(new Patient { Id = patientId, Name = "Bella", UserId = "Vhgfhb&%RTYHvbhu77*&TTYH" });
            
            var behandeling = new Behandeling
            {
                Type = "Longtest",
                Arts = "",
                Datum = DateTime.Now.AddDays(1),
                Locatie = "Ziekenhuis"
            };

            // Act
            var result = await behandelingController.AddAsync(patientId, behandeling);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));

            behandelingRepository.Verify(x => x.InsertAsync(It.IsAny<Behandeling>()),Times.Once);
        }

        [TestMethod]
        public async Task AddBehandeling_OnlyDate_ReturnsCreated()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("Vhgfhb&%RTYHvbhu77*&TTYH");

            var patientId = Guid.NewGuid();
            patientRepository.Setup(x => x.SelectAsync(patientId)).ReturnsAsync(new Patient { Id = patientId, Name = "Bella", UserId = "Vhgfhb&%RTYHvbhu77*&TTYH"});

            var behandeling = new Behandeling
            {
                Type = "",
                Arts = "",
                Datum = DateTime.Now.AddDays(1),
                Locatie = ""
            };

            // Act
            var result = await behandelingController.AddAsync(patientId, behandeling);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));

            behandelingRepository.Verify(x => x.InsertAsync(It.IsAny<Behandeling>()), Times.Once);
        }

        [TestMethod]
        public async Task AddBehandeling_NotLoggenIn_ReturnsUnauthorized()
        {
            // Arrange
            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns((string?)null);

            var patientId = Guid.NewGuid();

            var behandeling = new Behandeling
            {
                Type = "Longtest",
                Arts = null,
                Datum = DateTime.Now.AddDays(1),
                Locatie = "Ziekenhuis"
            };

            // Act
            var result = await behandelingController.AddAsync(patientId, behandeling);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedResult));

            behandelingRepository.Verify(x => x.InsertAsync(It.IsAny<Behandeling>()),Times.Never);

            patientRepository.Verify(x => x.SelectAsync(It.IsAny<Guid>()),Times.Never);
        }

    }
    
}