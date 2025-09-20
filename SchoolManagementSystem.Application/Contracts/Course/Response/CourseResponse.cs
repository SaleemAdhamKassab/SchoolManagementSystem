namespace SchoolManagementSystem.Application.Contracts.Course.Response
{
	public class CourseResponse
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int TeacherId { get; set; }
		public string TeacherName { get; set; } = string.Empty;
	}
}