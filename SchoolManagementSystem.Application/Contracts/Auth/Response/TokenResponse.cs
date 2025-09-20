namespace SchoolManagementSystem.Application.Contracts.Auth.Response
{
	public class TokenResponse
	{
		public string Token { get; set; } = string.Empty;
		public DateTime ExpiresIn { get; set; }
	}
}