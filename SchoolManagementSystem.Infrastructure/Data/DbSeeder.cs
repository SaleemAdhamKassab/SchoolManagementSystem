using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces.Repositories;

namespace SchoolManagementSystem.Infrastructure.Data
{
	public class DbSeeder(IAuthRepository authRepository)
	{
		private readonly IAuthRepository _authRepository = authRepository;

		public async Task SeedAdminAsync()
		{
			if (!await _authRepository.UserExistsAsync("Admin@school.com", CancellationToken.None))
			{
				var adminUser = new User
				{
					FirstName = "Admin",
					LastName = "Admin",
					Email = "Admin@school.com",
					Role = EnmUserRole.Admin.ToString()
				};

				await _authRepository.RegisterAsync(adminUser, "P@ssw0rd123@", CancellationToken.None);
			}
		}
	}
}