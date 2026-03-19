using GameBackend.Models;
using GameBackend.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Route("environment2d/{environmentId}/object2d")]
    public class BehandelingController : ControllerBase
    {
        private readonly IBehandelingRepository _behandelingRepository;
        private readonly IAuthenticationService _authenticationService;

        public BehandelingController(IBehandelingRepository behandelingRepository, IAuthenticationService authenticationService)
        {
            this._behandelingRepository = behandelingRepository;
            this._authenticationService = authenticationService;
        }

        [HttpGet(Name = "GetAllBehandelingenByPatientId")]
        public async Task<IActionResult> GetByPatient(Guid patientId)
        {

            var behandelingen = await _behandelingRepository.SelectByPatientAsync(patientId);
            return Ok(behandelingen);
        }

        [HttpPost(Name = "AddBehandeling")]
        public async Task<ActionResult<Behandeling>> AddAsync(Guid patientId, Behandeling behandeling)
        {
            behandeling.Id = Guid.NewGuid();
            behandeling.PatientId = patientId;
            Console.WriteLine(behandeling.PatientId);

            await _behandelingRepository.InsertAsync(behandeling);

            return CreatedAtAction(nameof(GetByPatient), new { patientId }, behandeling);
        }
    }
}
