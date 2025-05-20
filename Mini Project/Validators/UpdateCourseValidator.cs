using FluentValidation;
using Mini_Project.Dtos;

namespace Mini_Project.Validators
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseDto>
    {
        public UpdateCourseValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(10).WithMessage("Please write the course description");
        }
    }
}
