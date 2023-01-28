using KRSPurchase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRSPurchase.ApplicationService
{
    
    public class SupplierApplicationService 
    {
        private readonly SupplierValidator _supplierValidator = new();

        private IList<Supplier> _supplierList = new List<Supplier>()
        {
            new Supplier ("PNPAY", "PicknPay", 1),
            new Supplier ("CNA01", "CNA", 1),
            new Supplier("LOGIC","Logic",12),
            new Supplier ("LNOVO","Lenoooovo", 2),
    };

        public async Task<IList<Supplier>> ListAsync()
        {
            return _supplierList;
        }
        public async Task<Supplier> FindByCodeAsync(string code)
        {
            return _supplierList.FirstOrDefault(sp => sp.Code == code)!;
        }

        public async Task<bool> AddAsync(Supplier supplier)
        {
            if (await CheckDuplicateAysnc(supplier.Code)) return false;
            var validationResult = _supplierValidator.Validate(supplier);
            if (!validationResult.IsValid) return false;

            _supplierList.Add(supplier);
            return true;
        }

        public async Task<bool> DeleteAsync(string code) 
        {
            var supplier = await FindByCodeAsync(code);
            if (supplier == null) return false;

            _supplierList.Remove(supplier);
            return true;
        }
        public async Task<bool> CheckDuplicateAysnc(string code)
        {
            var supplierDuplicate = await FindByCodeAsync(code);

            return supplierDuplicate != null;

        }
        public async Task<bool> EditAsync(Supplier existingSupplier)
        {
            var supplier = await FindByCodeAsync(existingSupplier.Code);
            var validationResult = _supplierValidator.Validate(supplier);
            if (validationResult.IsValid)
            {
                _supplierList = _supplierList.Select(sp => sp.Code == existingSupplier.Code ? existingSupplier : sp).ToList();
                return true;
            }
            return false;
        }
    }
}
