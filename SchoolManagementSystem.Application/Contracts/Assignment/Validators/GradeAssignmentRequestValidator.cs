using FluentValidation;
using SchoolManagementSystem.Application.Contracts.Assignment.Request;

namespace SchoolManagementSystem.Application.Contracts.Assignment.Validators
{
	public class GradeAssignmentRequestValidator : AbstractValidator<GradeAssignmentRequest>
	{
		public GradeAssignmentRequestValidator()
		{
			RuleFor(x => x.Grade)
			.InclusiveBetween(0, 100).WithMessage("Grade must be between 0 and 100");
		}
	}
}