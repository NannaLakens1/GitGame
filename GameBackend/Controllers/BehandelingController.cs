using GameBackend.Models;
using GameBackend.Repositories;
//using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using GameBackend.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameBackend.Controllers
{
    [ApiController]
    [Route("patients/{patientId}/behandeling")]
    public class BehandelingController : ControllerBase
    {
        private readonly IBehandelingRepository _behandelingRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPatientRepository _patientRepository;

        public BehandelingController(IBehandelingRepository behandelingRepository, IAuthenticationService authenticationService, IPatientRepository patientRepository)
        {
            this._behandelingRepository = behandelingRepository;
            this._authenticationService = authenticationService;
            this._patientRepository = patientRepository;
        }

        // alle behandelingen van een patient ophalen
        [HttpGet]
        public async Task<IActionResult> GetByPatient(Guid patientId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var patient = await _patientRepository.SelectAsync(patientId);

            if (patient == null)
            {
                return NotFound();
            }

            if (patient.UserId != userId)
            {
                return Unauthorized();
            }

            var behandelingen = await _behandelingRepository.SelectByPatientAsync(patientId);
            return Ok(behandelingen);
        }

        // behandeling bij een patient toevoegen
        [HttpPost]
        public async Task<ActionResult<Behandeling>> AddAsync(Guid patientId, Behandeling behandeling)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var patient = await _patientRepository.SelectAsync(patientId);

            if (patient == null)
            {
                return NotFound();
            }

            if (patient.UserId != userId)
            {
                return Unauthorized();
            }

            behandeling.Id = Guid.NewGuid();
            behandeling.PatientId = patientId;

            await _behandelingRepository.InsertAsync(behandeling);

            return CreatedAtAction(nameof(GetByPatient), new { patientId }, behandeling);
        }
    }
}
