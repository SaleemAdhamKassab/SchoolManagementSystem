using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces.Repositories;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.Infrastructure.Repositories
{
	public class AuthRepository(ApplicationDbContext applicationDbContext) : IAuthRepository
	{
		private readonly ApplicationDbContext _context = applicationDbContext;

		public async Task<User> RegisterAsync(User user, string password, CancellationToken cancellationToken)
		{
			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

			await _context.Users.AddAsync(user,cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return user;
		}
		public async Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);

			if (user == null) return null;

			// Verify the provided password against the hashed password
			if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null; // Password mismatch

			return user;
		}

		public async Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken)
		{
			return await _context.Users.AnyAsync(u => u.Email.Trim().ToLower() == email.Trim().ToLower(), cancellationToken);
		}

		public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
		}
	}
}