using GameBackend.Models;
using GameBackend.Repositories;
using GameBackend.Services;
//using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GameBackend.Controllers
{
    [ApiController]
    [Route("[controller]")] //dat de url dus environment2d wordt
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAuthenticationService _authenticationService;

        public PatientController(IPatientRepository patientRepository, IAuthenticationService authenticationService)
        {
            this._patientRepository = patientRepository;
            this._authenticationService = authenticationService;
        }

        [HttpGet(Name = "GetPatient")]
        public async Task<ActionResult<List<Patient>>> GetAsync()
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            var userPatients = await _patientRepository.SelectAsyncByUserId(userId);
            return Ok(userPatients);
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
        

        [HttpPost(Name = "AddPatient")]
        public async Task<ActionResult<Patient>> AddAsync(Patient patient)
        {
            if (string.IsNullOrWhiteSpace(patient.Name) || patient.Name.Length > 25)
            {
                return BadRequest("Environment name must be in between 1 or 25 karakters");
            }

            patient.Id = Guid.NewGuid();
            patient.UserId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (string.IsNullOrWhiteSpace(patient.UserId))
            {
                return BadRequest("You must be logged in to create an environment.");
            }

            var userPatients = await _patientRepository.SelectAsyncByUserId(patient.UserId);

            if (userPatients.Count() >= 5)
            {
                return BadRequest("You cannot have more than 5 environments.");
            }

            if (userPatients.Any(e => e.Name == patient.Name))
            {
                return BadRequest("An environment with this name already exists.");
            }

            await _patientRepository.InsertAsync(patient);

            return Ok(patient);
            //return CreatedAtRoute("GetEnvironment2DById", new { environment2DId = environment2D.Id }, environment2D);
        }

        [HttpPut("{patientId}", Name = "UpdatePatient2D")]
        public async Task<ActionResult<Patient>> UpdateAsync(Guid patientId, Patient patient)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            var existingPatient = await _patientRepository.SelectAsync(patientId);

            if (existingPatient == null || userId != existingPatient.UserId)
                return NotFound(new ProblemDetails { Detail = $"Environment2D {patientId} not found" });

            if (patient.Id != patientId)
                return Conflict(new ProblemDetails { Detail = "The id of the Environment2D in the route does not match the id of the Environment2D in the body" });

            await _patientRepository.UpdateAsync(patient);

            return Ok(patient);
        }

        [HttpDelete("{patientId}", Name = "DeletePatient")]
        public async Task<ActionResult> DeleteAsync(Guid patientId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            var patient = await _patientRepository.SelectAsync(patientId);

            if (patient == null || userId != patient.UserId)
                return NotFound(new ProblemDetails { Detail = $"Environment2D {patientId} not found" });

            await _patientRepository.DeleteAsync(patientId);

            return Ok();
        }

    }
}
