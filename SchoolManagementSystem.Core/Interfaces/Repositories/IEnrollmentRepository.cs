using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.API.Repositories.EnrollmentRepository
{
	public interface IEnrollmentRepository
	{
		Task<Enrollment?> AddEnrollmentAsync(Enrollment enrollment, CancellationToken cancellationToken);
		Task<bool> IsStudentEnrolledAsync(int studentId, int courseId, CancellationToken cancellationToken);
	}
}