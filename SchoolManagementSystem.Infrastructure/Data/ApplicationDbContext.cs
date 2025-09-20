using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Infrastructure.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }
		public DbSet<Assignment> Assignments { get; set; }
		public DbSet<Grade> Grades { get; set; }
		public DbSet<StudentAssignmentSubmission> StudentAssignmentSubmissions { get; set; }
	}
}