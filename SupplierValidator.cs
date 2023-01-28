using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRSPurchase.Domain
{
    public class SupplierValidator : AbstractValidator<Supplier>
    {
        public SupplierValidator()
        {
            RuleFor(sp => sp.Code).NotEmpty().MinimumLength(5).MaximumLength(5);
            RuleFor(sp => sp.Name).NotEmpty();
            RuleFor(sp => sp.LeadTime).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
