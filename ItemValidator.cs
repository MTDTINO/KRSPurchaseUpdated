using FluentValidation;

namespace KRSPurchase.Domain
{
    public class ItemValidator:AbstractValidator<Item>
    {
       public ItemValidator() 
        {
            RuleFor(i => i.Good).NotNull().NotEmpty().SetValidator(new GoodValidator());
            RuleFor(i => i.Quantity).NotNull().NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(i => i.Price).GreaterThanOrEqualTo(0);
        }
    }
}
