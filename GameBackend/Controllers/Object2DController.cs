using GameBackend.Models;
using GameBackend.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameBackend.Controllers
{
    [ApiController]
    [Route("environment2d/{environmentId}/object2d")]
    public class Object2DController : ControllerBase
    {
        private readonly IBehandelingRepository _object2DRepository;
        private readonly IAuthenticationService _authenticationService;

        public Object2DController(IBehandelingRepository object2DRepository, IAuthenticationService authenticationService)
        {
            this._object2DRepository = object2DRepository;
            this._authenticationService = authenticationService;
        }

        [HttpGet(Name = "GetAllObjects2DByEnvironmentId")]
        public async Task<IActionResult> GetByEnvironment(Guid environmentId)
        {

            var objects = await _object2DRepository.SelectByEnvironmentAsync(environmentId);
            return Ok(objects);
        }

        [HttpPost(Name = "AddObject2D")]
        public async Task<ActionResult<Behandeling>> AddAsync(Guid environmentId, Behandeling object2D)
        {
            object2D.Id = Guid.NewGuid();
            object2D.EnvironmentId = environmentId;
            Console.WriteLine(object2D.EnvironmentId);

            await _object2DRepository.InsertAsync(object2D);

            return CreatedAtAction(nameof(GetByEnvironment), new { environmentId }, object2D);
            //return CreatedAtRoute("GetObject2DById", new { object2DId = object2D.Id }, object2D);
        }

        [HttpPut("{object2DId}", Name = "UpdateObject2D")]
        public async Task<ActionResult<Behandeling>> UpdateAsync(Guid environmentId, Guid object2DId, Behandeling object2D)
        {
            var existingObject2D = await _object2DRepository.SelectAsync(object2DId, environmentId);

            if (existingObject2D == null)
                return NotFound(new ProblemDetails { Detail = $"Object2D {object2DId} not found" });

            if (object2D.Id != object2DId)
                return Conflict(new ProblemDetails { Detail = "The id of the Object2D in the route does not match the id of the Object2D in the body" });

            object2D.EnvironmentId = environmentId;

            await _object2DRepository.UpdateAsync(object2D);

            return Ok(object2D);
        }

        [HttpDelete("{object2DId}", Name = "DeleteObject2D")]
        public async Task<ActionResult> DeleteAsync(Guid environmentId, Guid object2DId)
        {
            var object2D = await _object2DRepository.SelectAsync(object2DId, environmentId);

            if (object2D == null)
                return NotFound(new ProblemDetails { Detail = $"Object2D {object2DId} not found" });

            await _object2DRepository.DeleteAsync(object2DId, environmentId);

            return Ok();
        }
    }
}
