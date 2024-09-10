using FluentValidation;
using GamingStore.GamingStore.Models.Requests;

namespace GamingStore.Validators
{
    public class RemoveGameTagRequestValidator : AbstractValidator<RemoveGameTagRequest>
    {
        public RemoveGameTagRequestValidator()
        {
            RuleFor(x=>x.Title).NotEmpty();
            RuleFor(x=>x.GameTag).NotEmpty();
        }
    }
}
