using FluentValidation;
using SchoolManagementSystem.Application.Contracts.Auth.Request;
using SchoolManagementSystem.Core.Constants;

namespace SchoolManagementSystem.Application.Contracts.Auth.Validators
{
	public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
	{
		public UserRegisterRequestValidator()
		{
			RuleFor(e => e.FirstName).NotEmpty().MaximumLength(100);
			RuleFor(e => e.LastName).NotEmpty().MaximumLength(100);
			RuleFor(e => e.Email).NotEmpty().EmailAddress();
			RuleFor(e => e.Password)
				.NotEmpty()
				.Matches(RegexPatterns.Password)
				.WithMessage("Password Rules: (8+ Digits , contains lowercase-uppercase Alphanumeric");
		}
	}
}