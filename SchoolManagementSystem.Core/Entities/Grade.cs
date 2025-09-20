using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Core.Entities
{
	public class Grade
	{
		public int Id { get; set; }
		public decimal Score { get; set; }
		public string Feedback { get; set; } = string.Empty;


		public int StudentId { get; set; }
		[ForeignKey("StudentId")]
		public User Student { get; set; } = default!;


		public int AssignmentId { get; set; }
		[ForeignKey("AssignmentId")]
		public Assignment Assignment { get; set; } = default!;
	}
}