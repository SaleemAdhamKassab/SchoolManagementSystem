using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.API.Services.EnrollmentService;
using SchoolManagementSystem.Application.Contracts.Enrollment.Request;

namespace SchoolManagementSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
	{
		private readonly IEnrollmentService _enrollmentService = enrollmentService;

		[HttpPost]
		public async Task<IActionResult> EnrollStudent(EnrollmentRequest request, CancellationToken cancellationToken)
		{
			return Ok(await _enrollmentService.EnrollStudentAsync(request, cancellationToken));
		}
	}
}