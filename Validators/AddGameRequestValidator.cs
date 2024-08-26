using FluentValidation;
using GamingStore.GamingStore.Models.Requests;

namespace GamingStore.Validators
{
    public class AddGameRequestValidator : AbstractValidator<AddGameRequest>
    {
        public AddGameRequestValidator()
        { 
            RuleFor(x=> x.Title).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.ReleaseDate).NotEmpty().GreaterThan(new DateTime(1950, 01, 01));
            RuleFor(x => x.GameTags).NotEmpty();
        }
    }
}
