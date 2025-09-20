using FluentValidation;
using SchoolManagementSystem.Application.Contracts.Assignment.Request;

namespace SchoolManagementSystem.Application.Contracts.Assignment.Validators
{
	public class AssignmentRequestValidator : AbstractValidator<CreateAssignmentRequest>
	{
		public AssignmentRequestValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required");

			RuleFor(x => x.DueDate)
				.NotEmpty().WithMessage("Due date is required")
				.GreaterThan(DateTime.Today).WithMessage("Due date must be in the future");

			RuleFor(x => x.CourseId)
				.NotEmpty().WithMessage("Course ID is required");
		}
	}
}