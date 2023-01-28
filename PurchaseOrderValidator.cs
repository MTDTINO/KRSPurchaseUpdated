using FluentValidation;

namespace KRSPurchase.Domain
{
    public class PurchaseOrderValidator:AbstractValidator<PurchaseOrder>
    {
        public PurchaseOrderValidator()
        {
            RuleFor(po => po.Number).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(po => po.OrderDate).NotNull().NotEmpty();
            RuleFor(po => po.Supplier).NotNull().NotEmpty().SetValidator(new SupplierValidator());
            RuleForEach(po => po.Items).NotNull().NotEmpty().SetValidator(new ItemValidator());
        }
    }
}
