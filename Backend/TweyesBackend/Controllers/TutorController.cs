using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweyesBackend.Core.Contract;
using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;

namespace TweyesBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Tutor")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorService _tutorService;
        public TutorController(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }
        [Authorize]
        [HttpPost("addTutor")]
        public async Task<ActionResult<Response<AddTutorResponse>>> AddPayer([FromBody] AddTutorRequest request)
        {
            var response = await _tutorService.AddTutor(request);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("GetTutor")]
        public async Task<ActionResult<Response<AddTutorResponse>>> GetPayer()
        {
            var response = await _tutorService.GetTutor();
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("GetTutorById/{id}")]
        public async Task<ActionResult<Response<AddTutorResponse>>> GetPayerById(string id)
        {
            var response = await _tutorService.GetById(id);
            return Ok(response);
        }

       // [Authorize(Roles = "Administrator")]
        [HttpPost("GetAllTutor")]
        public async Task<ActionResult<Response<AddTutorResponse>>> GetAllPayers()
        {
            var response = await _tutorService.GetAllTutor();
            return Ok(response);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("deleteTutor/{id}")]
        public async Task<ActionResult<Response<AddTutorResponse>>> DeletePayer(string id)
        {
            var response = await _tutorService.SoftDeletePayer(id);
            return Ok(response);
        }

        [HttpPost("updateTutor")]
        public async Task<ActionResult<Response<AddTutorResponse>>> UpdateStudent([FromBody] UpdateTutorRequest request)
        {

            var response = await _tutorService.UpdateTutor(request);
            return Ok(response);


        }

    }
}
