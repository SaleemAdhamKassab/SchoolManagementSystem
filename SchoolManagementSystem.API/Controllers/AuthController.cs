using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Contracts.Auth.Request;
using SchoolManagementSystem.Application.Interfaces.Services;

namespace SchoolManagementSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService) : ControllerBase
	{

		private readonly IAuthService _authService = authService;

		[HttpPost("register")]
		public async Task<IActionResult> Register(UserRegisterRequest userDto, CancellationToken cancellationToken)
		{
			return Ok(await _authService.RegisterAsync(userDto, cancellationToken));
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login(UserLoginRequest userDto, CancellationToken cancellationToken)
		{
			return Ok(await _authService.LoginAsync(userDto, cancellationToken));
		}
	}
}