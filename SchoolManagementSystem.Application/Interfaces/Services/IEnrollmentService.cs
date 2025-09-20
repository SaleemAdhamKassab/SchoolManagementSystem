using SchoolManagementSystem.Application.Contracts.Common;
using SchoolManagementSystem.Application.Contracts.Enrollment.Request;

namespace SchoolManagementSystem.API.Services.EnrollmentService
{
	public interface IEnrollmentService
	{
		Task<GeneralResponse<bool>> EnrollStudentAsync(EnrollmentRequest request, CancellationToken cancellationToken);
	}
}