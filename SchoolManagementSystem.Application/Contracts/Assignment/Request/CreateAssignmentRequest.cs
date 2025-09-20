namespace SchoolManagementSystem.Application.Contracts.Assignment.Request
{
	public class CreateAssignmentRequest
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime DueDate { get; set; }
		public int CourseId { get; set; }
	}
}
