using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces.Repositories;
using SchoolManagementSystem.Infrastructure.Data;
using System.Linq.Dynamic.Core;

namespace SchoolManagement.Repositories
{
	public class CourseRepository(ApplicationDbContext context) : ICourseRepository
	{
		private readonly ApplicationDbContext _context = context;


		public async Task<Course?> GetCourseByIdAsync(int id, CancellationToken cancellationToken)
		{
			return await _context.Courses.FindAsync(id, cancellationToken);
		}
		public IQueryable<Course>? GetAllCoursesQuery(int userId, string userRole, string searchValue, string sortColumn, string sortDirection, CancellationToken cancellationToken)
		{
			var query = _context.Courses.Include(e => e.Teacher).AsQueryable();

			if (userRole == EnmUserRole.Student.ToString() && userId > 0)
			{
				query = query.Where(e => e.Enrollments.Any(e => e.StudentId == userId));
			}

			searchValue = !string.IsNullOrEmpty(searchValue) ? searchValue.Trim().ToLower() : searchValue;

			if (!string.IsNullOrEmpty(searchValue))
				query = query.Where(e => e.Title.Trim().ToLower().Contains(searchValue) ||
										 e.Description.Trim().ToLower().Contains(searchValue) ||
										 e.Teacher.FirstName.Trim().ToLower().Contains(searchValue) ||
										 e.Teacher.LastName.Trim().ToLower().Contains(searchValue));

			if (!string.IsNullOrEmpty(sortColumn))
				query = query.OrderBy($"{sortColumn} {sortDirection}");

			return query;
		}
		public async Task<Course> AddCourseAsync(Course course, CancellationToken cancellationToken)
		{
			await _context.Courses.AddAsync(course, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return course;
		}
		public async Task<bool> UpdateCourseAsync(Course course, CancellationToken cancellationToken)
		{
			_context.Courses.Update(course);
			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}
		public async Task<bool> DeleteCourseAsync(int id, CancellationToken cancellationToken)
		{
			var course = await _context.Courses.FindAsync(id, cancellationToken);
			if (course == null) return false;

			_context.Courses.Remove(course);
			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}
		public async Task<bool> IsTeacherValidAsync(int teacherId, CancellationToken cancellationToken)
		{
			return await _context.Users.AnyAsync(u => u.Id == teacherId && u.Role == EnmUserRole.Teacher.ToString(), cancellationToken);
		}
		public async Task<bool> IsStudentEnrolledInCourseAsync(int courseId, int studentId, CancellationToken cancellationToken)
		{
			return  await _context.Enrollments.AnyAsync(e => e.CourseId == courseId && e.StudentId == studentId, cancellationToken);
		}
	}
}