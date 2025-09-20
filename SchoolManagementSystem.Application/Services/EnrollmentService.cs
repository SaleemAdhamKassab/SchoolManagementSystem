using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.API.Repositories.EnrollmentRepository;
using SchoolManagementSystem.API.Services.EnrollmentService;
using SchoolManagementSystem.Application.Contracts.Common;
using SchoolManagementSystem.Application.Contracts.Enrollment.Request;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces.Repositories;

namespace SchoolManagementSystem.Application.Services
{
	public class EnrollmentService(IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository, IAuthRepository authRepository) : IEnrollmentService
	{
		private readonly IEnrollmentRepository _enrollmentRepository = enrollmentRepository;
		private readonly ICourseRepository _courseRepository = courseRepository;
		private readonly IAuthRepository _authRepository = authRepository;

		public async Task<GeneralResponse<bool>> EnrollStudentAsync(EnrollmentRequest request, CancellationToken cancellationToken)
		{
			var course = await _courseRepository.GetCourseByIdAsync(request.CourseId, cancellationToken);

			if (course == null)
				return new GeneralResponse<bool>(false, "Course not found", false, StatusCodes.Status404NotFound);

			var student = await _authRepository.GetUserByIdAsync(request.StudentId, cancellationToken);

			if (student == null || student.Role != EnmUserRole.Student.ToString())
				return new GeneralResponse<bool>(false, "Student not found.", false, StatusCodes.Status404NotFound);

			if (await _enrollmentRepository.IsStudentEnrolledAsync(request.StudentId, request.CourseId, cancellationToken))
				return new GeneralResponse<bool>(false, "Student is already enrolled in this course", false, StatusCodes.Status409Conflict);

			var enrollment = new Enrollment
			{
				StudentId = request.StudentId,
				CourseId = request.CourseId
			};

			var result = await _enrollmentRepository.AddEnrollmentAsync(enrollment, cancellationToken);

			if (result == null)
				return new GeneralResponse<bool>(false, "Enrollment failed", false, StatusCodes.Status500InternalServerError);

			return new GeneralResponse<bool>(true, "Enrollment successful", true, StatusCodes.Status200OK);
		}
	}
}