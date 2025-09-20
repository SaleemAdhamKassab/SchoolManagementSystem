using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.API.Extensions;
using SchoolManagementSystem.Application.Contracts.Assignment.Request;
using SchoolManagementSystem.Application.Interfaces.Services;

namespace SchoolManagementSystem.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class AssignmentsController(IAssignmentService assignmentService) : ControllerBase
	{
		private readonly IAssignmentService _assignmentService = assignmentService;


		[Authorize(Roles = "Teacher")]
		[HttpPost]
		public async Task<IActionResult> AddAssignment(CreateAssignmentRequest request, CancellationToken cancellationToken)
		{
			var userId = int.Parse(User.GetUserId()!);

			return Ok(await _assignmentService.AddAssignmentAsync(request, userId, cancellationToken));
		}


		[Authorize(Roles = "Student")]
		[HttpPost("studentSubmit")]
		public async Task<IActionResult> AddAssignmentSubmission(AssignmentSubmissionRequest request, CancellationToken cancellationToken)
		{
			var userId = int.Parse(User.GetUserId()!);

			return Ok(await _assignmentService.AddAssignmentSubmissionAsync(request, userId, cancellationToken));
		}


		[Authorize(Roles = "Teacher")]
		[HttpPost("teacherGrade")]
		public async Task<IActionResult> GradeAssignment(GradeAssignmentRequest request, CancellationToken cancellationToken)
		{
			var userId = int.Parse(User.GetUserId()!);

			return Ok(await _assignmentService.GradeAssignmentAsync(request,userId,cancellationToken));
		}
	}
}