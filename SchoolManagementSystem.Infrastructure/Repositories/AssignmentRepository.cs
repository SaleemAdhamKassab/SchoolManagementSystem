using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces.Repositories;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.Infrastructure.Repositories
{
	public class AssignmentRepository(ApplicationDbContext applicationDbContext) : IAssignmentRepository
	{
		private readonly ApplicationDbContext _context = applicationDbContext;


		public async Task<Assignment?> GetAssignmentByIdAsync(int id, CancellationToken cancellationToken)
		{
			return await _context.Assignments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
		}
		public async Task<Assignment> AddAssignment(Assignment assignment, CancellationToken cancellationToken)
		{
			await _context.Assignments.AddAsync(assignment, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return assignment;
		}
		public async Task<StudentAssignmentSubmission> AddStudentAssignmentSubmission(StudentAssignmentSubmission studentAssignmentSubmission, CancellationToken cancellationToken)
		{
			await _context.StudentAssignmentSubmissions.AddAsync(studentAssignmentSubmission, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return studentAssignmentSubmission;
		}
		public async Task<StudentAssignmentSubmission?> GetSubmissionByAssignmentAndStudentAsync(int assignmentId, int studentId, CancellationToken cancellationToken)
		{
			return await _context.StudentAssignmentSubmissions.FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId, cancellationToken);
		}
		public async Task<StudentAssignmentSubmission?> GetSubmissionByIdAsync(int submissionId, CancellationToken cancellationToken)
		{
			return await _context.StudentAssignmentSubmissions.FirstOrDefaultAsync(s => s.Id == submissionId, cancellationToken);
		}
		public async Task<bool> UpdateStudentAssignmentSubmission(StudentAssignmentSubmission studentAssignmentSubmission, CancellationToken cancellationToken)
		{
			_context.StudentAssignmentSubmissions.Update(studentAssignmentSubmission);
			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}
	}
}