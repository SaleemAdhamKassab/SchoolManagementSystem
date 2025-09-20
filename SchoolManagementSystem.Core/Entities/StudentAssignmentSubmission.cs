using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Core.Entities
{
	public class StudentAssignmentSubmission
	{
		public int Id { get; set; }
		public string Status { get; set; } = string.Empty;
		public DateTime SubmittedDate { get; set; }
		public double? Grade { get; set; }
		public string? TeacherFeedback { get; set; }
		public DateTime? GradedDate { get; set; }

		public int StudentId { get; set; }
		[ForeignKey("StudentId")]
		public User Student { get; set; } = default!;

		public int AssignmentId { get; set; }
		[ForeignKey("AssignmentId")]
		public Assignment Assignment { get; set; } = default!;
	}
}