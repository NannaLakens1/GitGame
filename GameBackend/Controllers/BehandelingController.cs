using GameBackend.Models;
using GameBackend.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameBackend.Controllers
{
    [ApiController]
    [Route("environment2d/{environmentId}/object2d")]
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
            //return CreatedAtRoute("GetObject2DById", new { object2DId = object2D.Id }, object2D);
        }

        [HttpPut("{object2DId}", Name = "UpdateObject2D")]
        public async Task<ActionResult<Behandeling>> UpdateAsync(Guid patientId, Guid behandelingId, Behandeling behandeling)
        {
            var existingBehandeling = await _behandelingRepository.SelectAsync(behandelingId, patientId);

            if (existingBehandeling == null)
                return NotFound(new ProblemDetails { Detail = $"Object2D {behandelingId} not found" });

            if (behandeling.Id != behandelingId)
                return Conflict(new ProblemDetails { Detail = "The id of the Object2D in the route does not match the id of the Object2D in the body" });

            behandeling.PatientId = patientId;

            await _behandelingRepository.UpdateAsync(behandeling);

            return Ok(behandeling);
        }

        [HttpDelete("{object2DId}", Name = "DeleteObject2D")]
        public async Task<ActionResult> DeleteAsync(Guid patientId, Guid behandelingId)
        {
            var behandeling = await _behandelingRepository.SelectAsync(behandelingId, patientId);

            if (behandeling == null)
                return NotFound(new ProblemDetails { Detail = $"Object2D {behandelingId} not found" });

            await _behandelingRepository.DeleteAsync(behandelingId, patientId);

            return Ok();
        }
    }
}
