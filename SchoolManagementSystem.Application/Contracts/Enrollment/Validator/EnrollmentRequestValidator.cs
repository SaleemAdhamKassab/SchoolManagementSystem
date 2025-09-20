using FluentValidation;
using SchoolManagementSystem.Application.Contracts.Enrollment.Request;

namespace SchoolManagementSystem.Application.Contracts.Enrollment.Validator
{
	public class EnrollmentRequestValidator : AbstractValidator<EnrollmentRequest>
	{
		public EnrollmentRequestValidator()
		{
			RuleFor(x => x.StudentId)
					.NotEmpty().WithMessage("StudentId is required");

			RuleFor(x => x.CourseId)
				.NotEmpty().WithMessage("CourseId is required");
		}
	}
}