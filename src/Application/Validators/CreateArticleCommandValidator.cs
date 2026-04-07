using FluentValidation;
using Application.Features.Articles.Commands;

namespace Application.Validators;
public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Reference).NotEmpty().Length(13).Matches("^[0-9]+$");
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PrixHT).GreaterThan(0);
        RuleFor(x => x.TypeArticle).NotEmpty();
        When(x => x.TypeArticle == "Alimentaire", () =>
        {
            RuleFor(x => x.DLC).NotNull();
            RuleFor(x => x.TypeVente).NotNull();
        });
        When(x => x.TypeArticle == "NonAlimentaire", () =>
        {
            RuleFor(x => x.Packaging).NotNull();
        });
    }
}
