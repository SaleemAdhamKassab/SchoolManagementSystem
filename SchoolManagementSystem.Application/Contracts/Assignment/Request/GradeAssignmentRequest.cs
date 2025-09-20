namespace SchoolManagementSystem.Application.Contracts.Assignment.Request
{
	public class GradeAssignmentRequest
	{
		public int StudentAssignmentSubmissionId { get; set; }
		public double Grade { get; set; }
		public string? TeacherFeedback { get; set; }
	}
}
