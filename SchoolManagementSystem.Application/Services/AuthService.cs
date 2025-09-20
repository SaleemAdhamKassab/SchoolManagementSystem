using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Application.Contracts.Auth.Request;
using SchoolManagementSystem.Application.Contracts.Auth.Response;
using SchoolManagementSystem.Application.Contracts.Common;
using SchoolManagementSystem.Application.Interfaces.Services;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagementSystem.Application.Services
{
	public class AuthService(IAuthRepository authRepository, IConfiguration configuration) : IAuthService
	{
		private readonly IAuthRepository _authRepository = authRepository;
		private readonly IConfiguration _config = configuration;

		private TokenResponse GenerateJwtToken(User user)
		{
			var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.Role, user.Role.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var expiryMinutes = _config.GetValue<int>("Jwt:ExpiryMinutes");
			var expires = DateTime.Now.AddMinutes(expiryMinutes);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = expires,
				SigningCredentials = creds,
				Issuer = _config.GetSection("Jwt:Issuer").Value,
				Audience = _config.GetSection("Jwt:Audience").Value
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return new TokenResponse
			{
				Token = tokenHandler.WriteToken(token),
				ExpiresIn = expires
			};
		}



		public async Task<GeneralResponse<AuthResponse>> RegisterAsync(UserRegisterRequest request, CancellationToken cancellationToken)
		{
			if (await _authRepository.UserExistsAsync(request.Email,cancellationToken))
				return new GeneralResponse<AuthResponse>(false, "Email already exists", null, StatusCodes.Status400BadRequest);

			var userToCreate = new User
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				Role = request.Role.ToString()
			};

			var createdUser = await _authRepository.RegisterAsync(userToCreate, request.Password,cancellationToken);

			var tokenResult = GenerateJwtToken(createdUser);

			var authResponse = new AuthResponse
			{
				Id = createdUser.Id,
				FirstName = createdUser.FirstName,
				LastName = createdUser.LastName,
				Email = createdUser.Email,
				Role = createdUser.Role,
				Token = tokenResult.Token,
				ExpiresIn = tokenResult.ExpiresIn,
			};

			return new GeneralResponse<AuthResponse>(true, "User created successfully", authResponse, StatusCodes.Status200OK);
		}
		public async Task<GeneralResponse<AuthResponse>> LoginAsync(UserLoginRequest request, CancellationToken cancellationToken)
		{
			var user = await _authRepository.LoginAsync(request.Email, request.Password,cancellationToken);

			if (user == null)
				return new GeneralResponse<AuthResponse>(false, "Invalid email or password", null, StatusCodes.Status400BadRequest);

			var tokenResult = GenerateJwtToken(user);

			var authResponse = new AuthResponse
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Role = user.Role,
				Token = tokenResult.Token,
				ExpiresIn = tokenResult.ExpiresIn,
			};

			return new GeneralResponse<AuthResponse>(true, "Login successfully", authResponse, StatusCodes.Status200OK);
		}
	}
}