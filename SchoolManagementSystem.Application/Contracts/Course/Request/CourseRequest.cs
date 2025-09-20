namespace SchoolManagementSystem.Application.Contracts.Course.Request
{
	public class CourseRequest
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int TeacherId { get; set; }
	}
}