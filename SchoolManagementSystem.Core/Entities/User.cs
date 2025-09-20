namespace SchoolManagementSystem.Core.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;


		public ICollection<Enrollment> Enrollments { get; set; } = [];
		public ICollection<Grade> Grades { get; set; } = [];
		public ICollection<Course> Courses { get; set; } = [];
		public ICollection<StudentAssignmentSubmission> StudentAssignmentSubmissions { get; set; } = [];
	}
}