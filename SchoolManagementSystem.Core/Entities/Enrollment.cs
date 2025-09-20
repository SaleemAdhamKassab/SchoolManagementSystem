using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Core.Entities
{
	public class Enrollment
	{
		public int Id { get; set; }
		public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

		public int StudentId { get; set; }
		[ForeignKey("StudentId")]
		public User User { get; set; } = default!;


		public int CourseId { get; set; }
		[ForeignKey("CourseId")]
		public Course Course { get; set; } = default!;
	}
}