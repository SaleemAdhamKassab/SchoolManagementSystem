namespace SchoolManagementSystem.Application.Contracts.Assignment.Response
{
	public class AssignmentResponse
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime DueDate { get; set; }
		public string Course { get; set; } = string.Empty;
	}
}