
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces.Repositories
{
	public interface IAuthRepository
	{
		Task<User> RegisterAsync(User user, string password, CancellationToken cancellationToken);
		Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken);
		Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken);
		Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
	}
}