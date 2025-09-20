using SchoolManagementSystem.Application.Contracts.Assignment.Request;
using SchoolManagementSystem.Application.Contracts.Assignment.Response;
using SchoolManagementSystem.Application.Contracts.Common;

namespace SchoolManagementSystem.Application.Interfaces.Services
{
	public interface IAssignmentService
	{
		Task<GeneralResponse<AssignmentResponse>> AddAssignmentAsync(CreateAssignmentRequest request, int teacherId, CancellationToken cancellationToken);
		Task<GeneralResponse<AssignmentSubmissionResponse>> AddAssignmentSubmissionAsync(AssignmentSubmissionRequest request, int studentId, CancellationToken cancellationToken);
		Task<GeneralResponse<GradeAssignmentResponse>> GradeAssignmentAsync(GradeAssignmentRequest request, int teacherId, CancellationToken cancellationToken);
	}
}