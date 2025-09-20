namespace SchoolManagementSystem.Application.Contracts.Assignment.Response
{
	public class GradeAssignmentResponse
	{
		public int Id { get; set; }
		public string Status { get; set; } = string.Empty;
		public DateTime SubmittedDate { get; set; }
		public double? Grade { get; set; }
		public string? TeacherFeedback { get; set; }
		public DateTime? GradedDate { get; set; }
		public string Assignment { get; set; } = string.Empty;
	}
}