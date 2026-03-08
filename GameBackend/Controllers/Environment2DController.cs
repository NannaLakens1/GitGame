using GameBackend.Models;
using GameBackend.Repositories;
using GameBackend.Services;
//using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GameBackend.Controllers
{
    [ApiController]
    [Route("[controller]")] //dat de url dus environment2d wordt
    public class Environment2DController : ControllerBase
    {
        private readonly IEnvironment2DRepository _environment2DRepository;
        private readonly IAuthenticationService _authenticationService;

        public Environment2DController(IEnvironment2DRepository environment2DRepository, IAuthenticationService authenticationService)
        {
            this._environment2DRepository = environment2DRepository;
            this._authenticationService = authenticationService;
        }
        //test voor commit en push

        [HttpGet(Name = "GetEnvironment2D")] //name= "pad"?? is dit dan nu wel of niet wat achter de / komt?
        public async Task<ActionResult<List<Environment2D>>> GetAsync()
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            var userEnvironments = await _environment2DRepository.SelectAsyncByUserId(userId);
            return Ok(userEnvironments);
        }

        //niet nodig denk ik?
        /*
        [HttpGet("{environment2DId}", Name = "GetEnvironment2DById")] // url = environment2d/1 (bv)
        public async Task<ActionResult<Environment2D>> GetByIdAsync(Guid environment2DId)
        {
            var environment2D = await _environment2DRepository.SelectAsync(environment2DId);

            if (environment2D == null)
                return NotFound(new ProblemDetails { Detail = $"Environment2D {environment2DId} not found" });

            return Ok(environment2D);
        }*/
        

        [HttpPost(Name = "AddEnvironment2D")]
        public async Task<ActionResult<Environment2D>> AddAsync(Environment2D environment2D)
        {
            if (string.IsNullOrWhiteSpace(environment2D.Name) || environment2D.Name.Length > 25)
            {
                return BadRequest("Environment name must be in between 1 or 25 karakters");
            }

            environment2D.Id = Guid.NewGuid();
            environment2D.UserId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (string.IsNullOrWhiteSpace(environment2D.UserId))
            {
                return BadRequest("You must be logged in to create an environment.");
            }

            var userEnvironments = await _environment2DRepository.SelectAsyncByUserId(environment2D.UserId);

            if (userEnvironments.Count() >= 5)
            {
                return BadRequest("You cannot have more than 5 environments.");
            }

            if (userEnvironments.Any(e => e.Name == environment2D.Name))
            {
                return BadRequest("An environment with this name already exists.");
            }
            //methode om te checken dat er niet meer dan 5 werelden worden aangemaakt

            await _environment2DRepository.InsertAsync(environment2D);

            return Ok(environment2D);
            //return CreatedAtRoute("GetEnvironment2DById", new { environment2DId = environment2D.Id }, environment2D);
        }

        [HttpPut("{environment2DId}", Name = "UpdateEnvironment2D")]
        public async Task<ActionResult<Environment2D>> UpdateAsync(Guid environment2DId, Environment2D environment2D)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            var existingEnvironment2D = await _environment2DRepository.SelectAsync(environment2DId);

            if (existingEnvironment2D == null || userId != existingEnvironment2D.UserId)
                return NotFound(new ProblemDetails { Detail = $"Environment2D {environment2DId} not found" });

            if (environment2D.Id != environment2DId)
                return Conflict(new ProblemDetails { Detail = "The id of the Environment2D in the route does not match the id of the Environment2D in the body" });

            await _environment2DRepository.UpdateAsync(environment2D);

            return Ok(environment2D);
        }

        [HttpDelete("{environment2DId}", Name = "DeleteEnvironment2D")]
        public async Task<ActionResult> DeleteAsync(Guid environment2DId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            var environment2D = await _environment2DRepository.SelectAsync(environment2DId);

            if (environment2D == null || userId != environment2D.UserId)
                return NotFound(new ProblemDetails { Detail = $"Environment2D {environment2DId} not found" });

            await _environment2DRepository.DeleteAsync(environment2DId);

            return Ok();
        }

    }
}
