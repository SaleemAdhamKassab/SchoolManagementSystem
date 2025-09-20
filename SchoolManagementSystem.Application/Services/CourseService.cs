using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.API.Contracts.Common;
using SchoolManagementSystem.API.Services.CourseService;
using SchoolManagementSystem.Application.Contracts.Common;
using SchoolManagementSystem.Application.Contracts.Course.Request;
using SchoolManagementSystem.Application.Contracts.Course.Response;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces.Repositories;

namespace SchoolManagementSystem.Application.Services
{
	public class CourseService(ICourseRepository courseRepository, IAuthRepository authRepository) : ICourseService
	{
		private readonly ICourseRepository _courseRepository = courseRepository;
		private readonly IAuthRepository _authRepository = authRepository;

		public async Task<GeneralResponse<CourseResponse>> GetCourseByIdAsync(int id, CancellationToken cancellationToken)
		{
			var course = await _courseRepository.GetCourseByIdAsync(id, cancellationToken);

			if (course == null)
				return new GeneralResponse<CourseResponse>(false, $"Course with ID {id} not found", null, StatusCodes.Status404NotFound);

			var teacher = await _authRepository.GetUserByIdAsync(course.TeacherId, cancellationToken);

			var responseDto = new CourseResponse
			{
				Id = course.Id,
				Title = course.Title,
				Description = course.Description,
				TeacherId = course.TeacherId,
				TeacherName = $"{teacher!.FirstName} {teacher!.LastName}"
			};
			return new GeneralResponse<CourseResponse>(true, "Course fetched", responseDto, StatusCodes.Status200OK);
		}
		public async Task<GeneralResponse<PaginatedList<CourseResponse>>> GetAllCoursesAsync(int userId, string userRole, RequestFilters filters, CancellationToken cancellationToken)
		{
			var query = _courseRepository.GetAllCoursesQuery(userId, userRole, filters.SearchValue, filters.SortColumn, filters.SortDirection, cancellationToken);

			var courses = query
				.Select(e => new CourseResponse
				{
					Id = e.Id,
					Title = e.Title,
					Description = e.Description,
					TeacherId = e.TeacherId,
					TeacherName = e.Teacher.FirstName + " " + e.Teacher.LastName
				})
				.AsNoTracking();

			var result = await PaginatedList<CourseResponse>.CreateAsync(courses, filters.PageNumber, filters.PageSize, cancellationToken);

			return new GeneralResponse<PaginatedList<CourseResponse>>(true, "Courses fetched successfully", result, StatusCodes.Status200OK);
		}
		public async Task<GeneralResponse<CourseResponse>> CreateCourseAsync(int userId, CourseRequest request, CancellationToken cancellationToken)
		{
			var user = await _authRepository.GetUserByIdAsync(userId, cancellationToken);

			if (user == null)
				return new GeneralResponse<CourseResponse>(false, "User not found.", null, StatusCodes.Status404NotFound);

			if (!await _courseRepository.IsTeacherValidAsync(request.TeacherId, cancellationToken))
				return new GeneralResponse<CourseResponse>(false, "Invalid teacher ID. Only a teacher can own a course", null, StatusCodes.Status400BadRequest);

			if (user.Role == EnmUserRole.Teacher.ToString() && user.Id != request.TeacherId)
				return new GeneralResponse<CourseResponse>(false, "Teachers can only create courses for themselves", null, StatusCodes.Status403Forbidden);

			var course = new Course
			{
				Title = request.Title,
				Description = request.Description,
				TeacherId = request.TeacherId
			};

			var createdCourse = await _courseRepository.AddCourseAsync(course, cancellationToken);

			var teacher = await _authRepository.GetUserByIdAsync(course.TeacherId, cancellationToken);

			var responseDto = new CourseResponse
			{
				Id = createdCourse.Id,
				Title = createdCourse.Title,
				Description = createdCourse.Description,
				TeacherId = createdCourse.TeacherId,
				TeacherName = $"{teacher!.FirstName} {teacher!.LastName}"
			};

			return new GeneralResponse<CourseResponse>(true, "Course created successfully", responseDto, StatusCodes.Status200OK);
		}
		public async Task<GeneralResponse<CourseResponse>> UpdateCourseAsync(int id, int userId, CourseRequest request, CancellationToken cancellationToken)
		{
			var existingCourse = await _courseRepository.GetCourseByIdAsync(id, cancellationToken);

			if (existingCourse == null)
				return new GeneralResponse<CourseResponse>(false, $"Course with ID {id} not found", null, StatusCodes.Status404NotFound);

			var user = await _authRepository.GetUserByIdAsync(userId, cancellationToken);

			if (user == null || (user.Role == EnmUserRole.Teacher.ToString() && existingCourse.TeacherId != user.Id))
				return new GeneralResponse<CourseResponse>(false, "You are not authorized to update this course", null, StatusCodes.Status403Forbidden);

			if (!await _courseRepository.IsTeacherValidAsync(request.TeacherId, cancellationToken))
				return new GeneralResponse<CourseResponse>(false, "Invalid teacher ID. Only a teacher can own a course", null, StatusCodes.Status400BadRequest);

			existingCourse.Title = request.Title;
			existingCourse.Description = request.Description;
			existingCourse.TeacherId = request.TeacherId;

			bool isSuccess = await _courseRepository.UpdateCourseAsync(existingCourse, cancellationToken);

			if (!isSuccess)
				return new GeneralResponse<CourseResponse>(false, "Failed to update course", null, StatusCodes.Status500InternalServerError);

			var teacher = await _authRepository.GetUserByIdAsync(existingCourse.TeacherId, cancellationToken);

			var responseDto = new CourseResponse
			{
				Id = existingCourse.Id,
				Title = existingCourse.Title,
				Description = existingCourse.Description,
				TeacherId = existingCourse.TeacherId,
				TeacherName = $"{teacher!.FirstName} {teacher!.LastName}"
			};

			return new GeneralResponse<CourseResponse>(true, "Course updated successfully", responseDto, StatusCodes.Status200OK);
		}
		public async Task<GeneralResponse<bool>> DeleteCourseAsync(int id, int userId, CancellationToken cancellationToken)
		{
			var existingCourse = await _courseRepository.GetCourseByIdAsync(id, cancellationToken);

			if (existingCourse == null)
				return new GeneralResponse<bool>(false, $"Course with ID {id} not found.", false, StatusCodes.Status404NotFound);

			var user = await _authRepository.GetUserByIdAsync(userId, cancellationToken);

			if (user == null || (user.Role == EnmUserRole.Teacher.ToString() && existingCourse.TeacherId != user.Id))
				return new GeneralResponse<bool>(false, "You are not authorized to delete this course", false, StatusCodes.Status403Forbidden);


			bool isSuccess = await _courseRepository.DeleteCourseAsync(id, cancellationToken);

			if (!isSuccess)
				return new GeneralResponse<bool>(false, $"Course with ID {id} failed to delete", false, StatusCodes.Status400BadRequest);

			return new GeneralResponse<bool>(true, "Course deleted successfully", true, StatusCodes.Status200OK);
		}
	}
}