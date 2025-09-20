using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Core.Entities
{
	public class Course
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;


		public int TeacherId { get; set; }
		[ForeignKey("TeacherId")]
		public User Teacher { get; set; } = default!;

		public ICollection<Enrollment> Enrollments { get; set; } = [];
		public ICollection<Assignment> Assignments { get; set; } = [];
	}
}