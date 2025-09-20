namespace SchoolManagementSystem.Application.Contracts.Assignment.Response
{
	public class AssignmentSubmissionResponse
	{
		public int Id { get; set; }
		public string Status { get; set; } = string.Empty;
		public DateTime SubmittedDate { get; set; }
		public string Student { get; set; } = string.Empty;
	}
}