using API.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BuggyController : ControllerBase
	{
		// -----------------------
		// Public endpoints
		// -----------------------

		[HttpGet("unauthorized")]
		public IActionResult GetUnauthorized()
		{
			// Explicitly returns 401
			return Unauthorized(new { message = "You are not authorized to access this resource." });
		}

		[HttpGet("badrequest")]
		public IActionResult GetBadRequest()
		{
			return BadRequest(new { message = "This is not a valid request." });
		}

		[HttpGet("notfound")]
		public IActionResult GetNotFound()
		{
			return NotFound(new { message = "Resource could not be found." });
		}

		[HttpGet("internalerror")]
		public IActionResult GetInternalError()
		{
			throw new Exception("This is a test exception.");
		}

		[HttpPost("validationerror")]
		public IActionResult GetValidationError(CreateProductDto product)
		{
			if (product == null || string.IsNullOrEmpty(product.Name))
			{
				// Return 400 if validation fails
				return BadRequest(new { message = "Product validation failed." });
			}

			return Ok(product);
		}

		// -----------------------
		// Protected endpoint
		// -----------------------

		[Authorize]
		[HttpGet("secret")]
		public IActionResult GetSecret()
		{
			// This will only be called if the user is authenticated
			if (!User.Identity?.IsAuthenticated ?? true)
				return Unauthorized();

			var name = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
			var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

			return Ok(new
			{
				message = $"Hello {name} with the id of {id}"
			});
		}
	}
}
