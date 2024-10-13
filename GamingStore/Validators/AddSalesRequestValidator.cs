using FluentValidation;
using GamingStore.GamingStore.Models.Requests;

namespace GamingStore.Validators
{
    public class AddSalesRequestValidator : AbstractValidator<AddOrderRequest>
    {
        public AddSalesRequestValidator()
        {           
            RuleFor(x => x.OrderDate).NotEmpty().GreaterThan(new DateTime(1900 - 01 - 01));
            RuleFor(x => x.GameId).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ClientName).NotEmpty();
            RuleFor(x=>x.ClientEmail).NotEmpty().EmailAddress();
        }
    }
}
