using GameBackend.Models;
using GameBackend.Repositories;
using GameBackend.Services;
//using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GameBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userPatients = await _patientRepository.SelectAsyncByUserId(userId);
            return Ok(userPatients);
        }

        [HttpPost(Name = "AddPatient")]
        public async Task<ActionResult<Patient>> AddAsync(Patient patient)
        {
            if (string.IsNullOrWhiteSpace(patient.Name) || patient.Name.Length > 25)
            {
                return BadRequest("Patient name must be in between 1 or 25 karakters");
            }

            patient.Id = Guid.NewGuid();
            patient.UserId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (string.IsNullOrWhiteSpace(patient.UserId))
            {
                return BadRequest("You must be logged in to create an environment.");
            }

            await _patientRepository.InsertAsync(patient);

            return Ok(patient);
        }
    }
}
