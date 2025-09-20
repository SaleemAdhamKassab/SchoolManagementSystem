using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces.Repositories
{
	public interface ICourseRepository
	{
		Task<Course?> GetCourseByIdAsync(int id, CancellationToken cancellationToken);
		IQueryable<Course>? GetAllCoursesQuery(int userId, string userRole, string searchValue, string sortColumn, string sortDirection, CancellationToken cancellationToken);
		Task<Course> AddCourseAsync(Course course, CancellationToken cancellationToken);
		Task<bool> UpdateCourseAsync(Course course, CancellationToken cancellationToken);
		Task<bool> DeleteCourseAsync(int id, CancellationToken cancellationToken);
		Task<bool> IsTeacherValidAsync(int teacherId, CancellationToken cancellationToken);
		Task<bool> IsStudentEnrolledInCourseAsync(int courseId, int studentId, CancellationToken cancellationToken);
	}
}