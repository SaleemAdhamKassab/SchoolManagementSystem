using SchoolManagementSystem.Application.Contracts.Auth.Request;
using SchoolManagementSystem.Application.Contracts.Auth.Response;
using SchoolManagementSystem.Application.Contracts.Common;

namespace SchoolManagementSystem.Application.Interfaces.Services
{
	public interface IAuthService
	{
		Task<GeneralResponse<AuthResponse>> RegisterAsync(UserRegisterRequest request, CancellationToken cancellationToken);
		Task<GeneralResponse<AuthResponse>> LoginAsync(UserLoginRequest request, CancellationToken cancellationToken);
	}
}