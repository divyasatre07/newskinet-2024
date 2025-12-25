using API.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();

		}
		[HttpGet("badrequest")]
		public IActionResult GetBadRequest()
		{
			return BadRequest("not a good request");

		}
		[HttpGet("notfound")]
		public IActionResult GetNotFound()
		{
			return NotFound();

		}
		[HttpGet("internalerror")]
		public IActionResult GetInternalError()
		{
			throw new Exception("This is the test exception");
		}


		[HttpPost("validationerror")]
		public IActionResult GetValidationError(CreateProductDto product)
		{
			return Ok();

		}



	}
}
