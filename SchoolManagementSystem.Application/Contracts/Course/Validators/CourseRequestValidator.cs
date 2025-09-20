using FluentValidation;
using SchoolManagementSystem.Application.Contracts.Course.Request;

namespace SchoolManagementSystem.Application.Contracts.Course.Validators
{
	public class CourseRequestValidator : AbstractValidator<CourseRequest>
	{
		public CourseRequestValidator()
		{
			RuleFor(x => x.Title)
			   .NotEmpty().WithMessage("Course title is required")
			   .MaximumLength(100).WithMessage("Course title cannot exceed 100 characters");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Course description is required")
				.MaximumLength(500).WithMessage("Course description cannot exceed 500 characters");
		}
	}
}