using FluentValidation;
using GamingStore.GamingStore.Models.Requests;

namespace GamingStore.Validators
{
    public class AddGameTagRequestValidator : AbstractValidator<AddGameTagRequest>
    {
        public AddGameTagRequestValidator()
        {
            RuleFor(x=>x.Title).NotEmpty();
            RuleFor(x=>x.GameTag).NotEmpty();
        }
    }
}
