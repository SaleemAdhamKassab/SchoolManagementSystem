using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.API.Contracts.Common;
using SchoolManagementSystem.API.Extensions;
using SchoolManagementSystem.API.Services.CourseService;
using SchoolManagementSystem.Application.Contracts.Course.Request;
using System.Security.Claims;

namespace SchoolManagementSystem.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CoursesController(ICourseService courseService) : ControllerBase
	{
		private readonly ICourseService _courseService = courseService;

		[Authorize(Roles = "Admin,Teacher,Student")]
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
		{
			var userId = int.Parse(User.GetUserId()!);
			var userRole = User.FindFirst(ClaimTypes.Role)!.Value;

			return Ok(await _courseService.GetAllCoursesAsync(userId, userRole, filters, cancellationToken));
		}

		
		[HttpGet("{id}")]
		[AllowAnonymous] // Anyone can view a specific course
		public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
		{
			return Ok(await _courseService.GetCourseByIdAsync(id, cancellationToken));
		}

		[Authorize(Roles = "Admin,Teacher")]
		[HttpPost]
		public async Task<IActionResult> Create(CourseRequest request, CancellationToken cancellationToken)
		{
			var userId = int.Parse(User.GetUserId()!);

			return Ok(await _courseService.CreateCourseAsync(userId, request, cancellationToken));
		}

		[Authorize(Roles = "Admin,Teacher")]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, CourseRequest request, CancellationToken cancellationToken)
		{
			var userId = int.Parse(User.GetUserId()!);

			return Ok(await _courseService.UpdateCourseAsync(id, userId, request, cancellationToken));
		}

		[Authorize(Roles = "Admin,Teacher")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
		{
			var userId = int.Parse(User.GetUserId()!);

			return Ok(await _courseService.DeleteCourseAsync(id, userId, cancellationToken));
		}
	}
}