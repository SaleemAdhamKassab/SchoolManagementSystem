using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.API.Repositories.EnrollmentRepository
{
	public class EnrollmentRepository(ApplicationDbContext context) : IEnrollmentRepository
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<Enrollment?> AddEnrollmentAsync(Enrollment enrollment, CancellationToken cancellationToken)
		{
			await _context.Enrollments.AddAsync(enrollment, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return enrollment;
		}

		public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId, CancellationToken cancellationToken)
		{
			return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId, cancellationToken);
		}
	}
}