/*using Microsoft.AspNetCore.Mvc;
using Moq;
using GameBackend.Controllers;
using GameBackend.Services;
using GameBackend.Repositories;
using GameBackend.Models;

namespace GameBackend.Tests;

[TestClass]
public sealed class Environment2DNameTest
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
    public async Task PostEnvironmentWithFaultyNameTest()
    {
        //arrange
        var environment = new Environment2D
        {
            Name = "" // lege naam
        };

        //act
        var result = await controller.AddAsync(environment);

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

        environment2dRepository.Verify(r => r.InsertAsync(It.IsAny<Environment2D>()), Times.Never);
    }
}*/
