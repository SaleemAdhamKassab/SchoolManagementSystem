using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces.Repositories
{
	public interface IAssignmentRepository
	{
		Task<Assignment?> GetAssignmentByIdAsync(int id, CancellationToken cancellationToken);
		Task<Assignment> AddAssignment(Assignment assignment, CancellationToken cancellationToken);
		Task<StudentAssignmentSubmission> AddStudentAssignmentSubmission(StudentAssignmentSubmission studentAssignmentSubmission, CancellationToken cancellationToken);
		Task<StudentAssignmentSubmission?> GetSubmissionByAssignmentAndStudentAsync(int assignmentId, int studentId, CancellationToken cancellationToken);
		Task<StudentAssignmentSubmission?> GetSubmissionByIdAsync(int submissionId, CancellationToken cancellationToken);
		Task<bool> UpdateStudentAssignmentSubmission(StudentAssignmentSubmission studentAssignmentSubmission, CancellationToken cancellationToken);
	}
}