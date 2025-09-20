using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Core.Entities
{
	public class Assignment
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime DueDate { get; set; }


		public int CourseId { get; set; }
		[ForeignKey("CourseId")]
		public Course Course { get; set; } = default!;

		public ICollection<Grade> Grades { get; set; } = [];
		public ICollection<StudentAssignmentSubmission> StudentAssignmentSubmissions { get; set; } = [];
	}
}