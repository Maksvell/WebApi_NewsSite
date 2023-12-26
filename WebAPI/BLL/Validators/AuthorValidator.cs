using BLL.DTOs;
using FluentValidation;

namespace BLL.Validators;

public class AuthorValidator : AbstractValidator<AuthorDTO>
{
    public AuthorValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotNull().NotEmpty();
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}
