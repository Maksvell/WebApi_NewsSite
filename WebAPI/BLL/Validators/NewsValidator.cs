using BLL.DTOs;
using FluentValidation;

namespace BLL.Validators;

public class NewsValidator : AbstractValidator<NewsDTO>
{
    public NewsValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty(); 
        RuleFor(x => x.Body).NotNull().NotEmpty();
        RuleFor(x => x.AuthorName).NotNull().NotEmpty();
        RuleFor(x => x.RubricName).NotNull().NotEmpty();
    }
}
