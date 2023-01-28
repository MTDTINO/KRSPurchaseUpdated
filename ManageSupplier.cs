using KRSPurchase.ApplicationService;
using KRSPurchase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRSPurchase.Test
{
    public class ManageSupplier
    {
        private readonly SupplierApplicationService _supplierService = new();
        private readonly SupplierValidator _supplierValidator = new();
        [Fact]
        public void ShouldCreateASupplier()

        {
            //given a Supplier Code "CHKRS", name "Checkers" and a leadtime of 1
            const string name = "CHKRS";
            const string code = "Checkers";
            const int leadtime = 1;

            //when we create the supplplier
            var supplier = new Supplier(code, name, leadtime);

            //then the supplier should exist with the Supplier Code "CHKRS", name "Checkers" and a leadtime of 1
            Assert.NotNull(supplier);
            Assert.Equal(name, supplier.Name);
            Assert.Equal(code, supplier.Code);
            Assert.Equal(leadtime, supplier.LeadTime);
        }

        [Fact]
        public async void ShouldFindSupplierByCode()
        {
            //given a supplier with the code "CNA01" and name "CNA"
            const string code = "CNA01";

            //when applications service containing a supplier with that code
            var supplier = await _supplierService.FindByCodeAsync(code);

            //then a supplier with that code should exist 
            Assert.NotNull(supplier);
            Assert.Equal(code, supplier.Code);
        }

        [Fact]
        public async Task ShouldGetAListofSuppliers()
        {
            //given an application service containing supplier with a name a code "PNPAY" 
            //when we get a list of supplier
            var suppliers = await _supplierService.ListAsync();

            //the we shoulddget a list of existing suppliers
            Assert.Contains(suppliers, sp => sp.Code == "PNPAY");
            Assert.True(suppliers.Count() > 1);
        }

        [Fact]
        public async Task ShouldAddSupplier()
        {
            //given a supplier with a code "SMSNG" with a name "Samsung" leadtime of 3 days
            var supplier = new Supplier("SMSNG", "Samsung", 3);
            //when the supplier is added
            var addSupplier = await _supplierService.AddAsync(supplier);
            //then the supplier is added sucessfully
            Assert.True(addSupplier);
        }

        [Fact]
        public async Task ShouldDeleteSupplierByCode()
        {
            //given a supplier code "TPLNK", a name "Supplier" and a leadtime of 2
            //as well 
            const string code = "TPLNK";
            const string name = "TP-Link";
            const int leadtime = 2;

            var supplierAdded = await _supplierService.AddAsync(new Supplier(code, name, leadtime));
            Assert.True(supplierAdded);

            //when we delete the supplier
            var supplierDeleted = await _supplierService.DeleteAsync(code);

            //then the supplier should be deleted
            Assert.True(supplierDeleted);
            var deletedSupplier = await _supplierService.FindByCodeAsync(code);
            Assert.Null(deletedSupplier);
        }

        [Fact]
        public void ShouldValidateSupplier()
        {
            //given a supplier code "WIMPY", name "Wimpy", leadtime 3
            var supplier = new Supplier("WIMPY", "Wimpy", 3);

            //when supplier is validated
            var validate = _supplierValidator.Validate(supplier);

            //then supplier should be valid
            Assert.True(validate.IsValid);
        }

        [Fact]
        public void ShouldFailToValidateCode()
        {
            //given a supplier with code "WIMPY" or name "Wimpy" and leadtime (-1 or 3)
            var supplier = new Supplier("WIM", "Wimpy", 1);
            //when we alidate a supplier 
            var validator = _supplierValidator.Validate(supplier);

            //then the supplier should be invalid
            Assert.False(validator.IsValid); 
        }

        [Fact]
        public void ShouldFailToValidateName()
        {
            //given a supplier with code "WIMPY" or name "Wimpy" and leadtime (-1 or 3)
            var supplier = new Supplier("WIMPY", "", 1);
            //when we validate a supplier 
            var validator = _supplierValidator.Validate(supplier);
            //then the supplier should be invalid
            Assert.False(validator.IsValid);
        }

        [Fact]
        public void ShouldFailToValidateLeadTime()
        {
            //given a supplier with code "WIMPY" or name "Wimpy" and leadtime (-1 or 3)
            var supplier = new Supplier("WIMPY", "Wimpy", -1);
            //when we alidate a supplier 
            var validator = _supplierValidator.Validate(supplier);
            //then the supplier should be invalid

            Assert.False(validator.IsValid);
        }

        [Fact]
        public async Task ShouldBeAbleToEditName()
        {
            //given a supplier with name, code and lead\time
            const string code = "LNOVO";
            const string name = "Lenovo";

            //const int leadtime = 2;
            //when we  find the supplier
            //abd edit
            var existingSupplier = await _supplierService.FindByCodeAsync(code);
            existingSupplier.Name = name;
            var supplierEdited = await _supplierService.EditAsync(existingSupplier);

            //then supplier should be edited

            var editedSupplier = await _supplierService.FindByCodeAsync(code);
            Assert.True(supplierEdited);
            Assert.Equal(name, editedSupplier.Name);
        }

        [Fact]
        public async Task ShouldBeAbleToEditLeadtime()
        {
            //given a supplier with name, code and leadtime
            const string code = "LNOVO";
            const int leadtime = 6;

            //when we  find the supplier
            //and edit
            var existingSupplier = await _supplierService.FindByCodeAsync(code);
            existingSupplier.LeadTime = leadtime;

            //then supplier should be edited
            var supplierEdited = await _supplierService.EditAsync(existingSupplier); ;
            var editedSupplier = await _supplierService.FindByCodeAsync(code);
            Assert.Equal(leadtime, editedSupplier.LeadTime);
        }

        [Fact]
        public async Task ShouldBeAbleToEditNameAndLeadtime()
        {
            //given a supplier with name, code and leadtime
            const string code = "LNOVO";
            const string name = "Lenovo";
            const int leadtime = 6;

            //when we  find the supplier
            //abd edit
            var existingSupplier = await _supplierService.FindByCodeAsync(code);
            existingSupplier.LeadTime = leadtime;
            existingSupplier.Name = name;

            var supplierEdited = await _supplierService.EditAsync(existingSupplier);

            //then supplier should be edited
            var editedSupplier = await _supplierService.FindByCodeAsync(code);

            Assert.Equal(leadtime, editedSupplier.LeadTime);
            Assert.Equal(name, editedSupplier.Name);
            Assert.True(supplierEdited);
        }

        [Fact]
        public async Task ShouldCheckForDuplicate() 
        {
            //given a supplier code "LOGIC", name "Logic" and a leadtime of 12
            // and a supplier application service containing a supplier with that code
            var supplier = new Supplier("LOGIC","Logic",12);

            //when we try to add the supplier
            var addSupplier = await _supplierService.AddAsync(supplier);

            //then the supplier should not be added
            Assert.False(addSupplier);
        }
    }
}
