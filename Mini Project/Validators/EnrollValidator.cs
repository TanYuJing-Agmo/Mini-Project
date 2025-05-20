using FluentValidation;
using Mini_Project.Dtos;

namespace Mini_Project.Validators
{
    public class EnrollValidator : AbstractValidator<EnrollDto>
    {
        public EnrollValidator()
        {
            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("CourseId is required");
        }
    }
}
