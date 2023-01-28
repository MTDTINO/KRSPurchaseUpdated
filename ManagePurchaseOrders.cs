using KRSPurchase.ApplicationService;
using KRSPurchase.Domain;

namespace KRSPurchase.Test
{
    public class ManagePurchaseOrders
    {
        private readonly ItemValidator _itemValidator = new();
        private readonly PurchaseOrderValidator _purchaseOrderValidator = new();
        private readonly PurchaseOrderApplicationService _purchaseOrderApplicationService = new();

        [Fact]
        public void ShouldCreatePurchaseOrder()
        {
            //given an item with a good, quantity and price
            //create a purchase order with that item with order number, orderDate, supplier and estimatedDelivery date
            const string goodCode = "STDLR";
            const string goodName = "Staedtler";
            var good = new Good(goodCode, goodName);

            const string supplierCode = "GAME1";
            const string supplierName = "Game";
            var supplier = new Supplier(supplierCode, supplierName, 2);

            var item = new Item(good, 12, 50);

            //when purchase order is created 
            var purchaseOrder = new PurchaseOrder(supplier, item);

            //then the purchase order should exist and estimated delivery date must be calculated
            DateTime estimateDelivery = purchaseOrder.EstimatedDeliveryDate;
            Assert.NotNull(purchaseOrder);
            Assert.Equal(goodCode, good.Code);
            Assert.Equal(goodName, good.Name);
            Assert.Equal(supplierCode, supplier.Code);
            Assert.Equal(supplierName, supplier.Name);
            Assert.Equal(estimateDelivery, purchaseOrder.EstimatedDeliveryDate);
        }

        [Fact]
        public void ShouldValidateItem()
        {
            //given a code "STCKY" and name "Sticky Paper"
            const string goodCode = "STCKY";
            const string goodName = "Sticky Paper";
            var good = new Good(goodCode, goodName);

            //when we validate the item
            var item = new Item(good, 4, 90m);
            var validationResult = _itemValidator.Validate(item);

            //then the item should be valid
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ShouldFailToValidateItemGood()
        {
            //given a code "LSCRN" and name "Laptop Screen"
            const string goodCode = "LN";
            const string goodName = "Laptop Screen";
            var good = new Good(goodCode, goodName);

            //when we validate the item
            var item = new Item(good, 10, 90m);
            var validationResult = _itemValidator.Validate(item);

            //then the item should be invalid
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ShouldFailToValidateItemQuantity()
        {
            //given a code "STCKY" and name "Laptop Screen"
            const string goodCode = "LSCRN";
            const string goodName = "Laptop Screen";
            var good = new Good(goodCode, goodName);

            //when we validate the item
            var item = new Item(good, -10, 90);
            var validationResult = _itemValidator.Validate(item);

            //then the item should be invalid
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ShouldFailToValidateItemPrice()
        {
            //given a code "STCKY" and name "Sticky Paper"
            const string goodCode = "LSCRN";
            const string goodName = "Laptop Screen";
            var good = new Good(goodCode, goodName);

            //when we validate the item
            var item = new Item(good, 10, -1m);
            var validationResult = _itemValidator.Validate(item);

            //then the item should be invalid
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ShouldValidatePurchaseOrder()
        {
            //given an order with number, orderDate, supplier, estimatedDeliverydate, and item list
            var supplier = new Supplier("CNVRS", "Converse", 3);

            var good = new Good("CARDS","Cards");
            var item = new Item(good, 3, 48.99m);
            var purchaseOrder = new PurchaseOrder(supplier, item);

            //when validate the purchase order
            var poValidate = _purchaseOrderValidator.Validate(purchaseOrder);

            //then the purchase should be valid 
            Assert.True(poValidate.IsValid);
        }

        [Fact]
        public void ShouldFailToValidatePurchaseOrderByNumber()
        {
            //given an order with number, orderDate, supplier, estimatedDeliverydate, and item list
            var supplier = new Supplier("", "Converse", 3);
            
            var good = new Good("CARDS", "Cards");
            var item = new Item(good, 3, 48.99m);
            var purchaseOrder = new PurchaseOrder(supplier, item);

            //when validate the purchase order
            var poValidate = _purchaseOrderValidator.Validate(purchaseOrder);

            //then the purchase should be invalid 
            Assert.False(poValidate.IsValid);
        }

        [Fact]
        public void ShouldFailToValidatePurchaseOrderBySupplier()
        {
            //given an order with number, orderDate, supplier, estimatedDeliverydate, and item list
            var supplier = new Supplier("", "Converse", 3);
           
            var good = new Good("CARDS", "Cards");
            var item = new Item(good, 3, 48.99m);
            var purchaseOrder = new PurchaseOrder(supplier, item);

            //when validate the purchase order
            var poValidate = _purchaseOrderValidator.Validate(purchaseOrder);

            //then the purchase should be invalid 
            Assert.False(poValidate.IsValid);
        }

        [Fact]
        public void ShouldFailToValidatePurchaseOrderByItem()
        {
            //given an order with number, orderDate, supplier, estimatedDeliverydate, and item list
            var supplier = new Supplier("CNVRS", "Converse", 3);
           
            var good = new Good("CARDS", "Cards");
            var item = new Item(good, 3, -48.99m);
            var purchaseOrder = new PurchaseOrder(supplier, item);

            //when validate the purchase order
            var poValidate = _purchaseOrderValidator.Validate(purchaseOrder);

            //then the purchase should be invalid 
            Assert.False(poValidate.IsValid);
        }

        [Fact]
        public async Task ShouldFindPurchaseOrderByNumber()
        {
            //given an purchase order with number
            const int POnumber = 1;
           
            //when a purchase orderis found
            var findPo = await _purchaseOrderApplicationService.FindByNumberAsync(POnumber);

            //then a purchase order should exist
            Assert.NotNull(findPo);
            Assert.Equal(POnumber, findPo.Number);
        }

        [Fact]
        public async Task ShouldAddPurchaseOrderByNumber()
        {
            //given a purchaseOrder with a number, dateOrder,good and supplier
            var supplier = new Supplier("PNPAY", "PicknPay", 1);
          
            var item = new Item(new Good("KYBRD", "Keyboard"), 5, 99.90m);
            var purchaseOrder = new PurchaseOrder( supplier, item);

            //when the purchaseOrder is added
            var addPo = await _purchaseOrderApplicationService.AddAsync(purchaseOrder);
            var findAddedPO = await _purchaseOrderApplicationService.FindByNumberAsync(purchaseOrder.Number);

            //then the order is added sucessfully
            Assert.NotNull(findAddedPO);
            Assert.True(addPo);
        }

        [Fact]
        public async Task ShouldCancelPurchaseOrder()
        {
            //Given a purchase order with purchase order number == 1
            // find the purchase order
            const int number = 1;
           var findPO = await _purchaseOrderApplicationService.FindByNumberAsync(number);

            //When we cancel the purchase order 
            var cancelPO = await _purchaseOrderApplicationService.CancelAsync(number);

            //then the order should be canceled
            //find the purchase order again, and assert that isCancelled == true)
            var canceledPo = await _purchaseOrderApplicationService.FindByNumberAsync(number);
            Assert.True(cancelPO);
            Assert.Equal(number, findPO.Number);
            Assert.Equal(number, canceledPo.Number);
            Assert.True(canceledPo.isCancelled);
        }

        [Fact]
        public async Task ShouldGetPurchaseOrderList()
        {
            //given an app service with at least 1 PO
            //when we fetch PurchaseOrder list
            var listPO = await _purchaseOrderApplicationService.ListAsync();
            //then that list should could contain at least 1 PO
            Assert.True(listPO.Count() > 1);
        }

        [Fact]
        public async Task ShouldAddItemToPurchaseOrderList() 
        {
            //given a purchaseOrder with a good and supplier
            var supplier = new Supplier("SMSNG", "Samsung", 2);

            var item = new Item(new Good("SPHNE", "Samsung Phone"), 3, 10090m);
            var purchaseOrder = new PurchaseOrder(supplier, item);

            //when the purchaseOrder with the same item exists
            var findAddedPO = await _purchaseOrderApplicationService.FindByNumberAsync(purchaseOrder.Number);
            var addPo = await _purchaseOrderApplicationService.AddItemAsync(findAddedPO, new List<Item>() { item});
          
            //then the quantity of that item is incremented
            Assert.True(addPo);
        }
    }

}
