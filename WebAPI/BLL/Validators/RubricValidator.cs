using BLL.DTOs;
using FluentValidation;

namespace BLL.Validators;

public class RubricValidator : AbstractValidator<RubricDTO>
{
    public RubricValidator()
    {
        RuleFor(r => r.Name).NotEmpty().NotNull();
    }
}
