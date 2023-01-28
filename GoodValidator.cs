using FluentValidation;
namespace KRSPurchase.Domain
{
    public class GoodValidator : AbstractValidator<Good>
    {
        public GoodValidator()
        {
            RuleFor(g => g.Code).NotEmpty().MinimumLength(5).MaximumLength(5);
            RuleFor(g => g.Name).NotEmpty();
        }
    }
}
