using SchoolManagementSystem.API.Contracts.Common;
using SchoolManagementSystem.Application.Contracts.Common;
using SchoolManagementSystem.Application.Contracts.Course.Request;
using SchoolManagementSystem.Application.Contracts.Course.Response;

namespace SchoolManagementSystem.API.Services.CourseService
{
	public interface ICourseService
	{
		Task<GeneralResponse<CourseResponse>> GetCourseByIdAsync(int id, CancellationToken cancellationToken);
		Task<GeneralResponse<PaginatedList<CourseResponse>>> GetAllCoursesAsync(int userId, string userRole, RequestFilters filters, CancellationToken cancellationToken);
		Task<GeneralResponse<CourseResponse>> CreateCourseAsync(int userId, CourseRequest request, CancellationToken cancellationToken);
		Task<GeneralResponse<CourseResponse>> UpdateCourseAsync(int id, int userId, CourseRequest request, CancellationToken cancellationToken);
		Task<GeneralResponse<bool>> DeleteCourseAsync(int id, int userId, CancellationToken cancellationToken);
	}
}