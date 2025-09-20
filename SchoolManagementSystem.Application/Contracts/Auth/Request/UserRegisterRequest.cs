using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Application.Contracts.Auth.Request
{
	public class UserRegisterRequest
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public EnmUserRole Role { get; set; }
	}
}