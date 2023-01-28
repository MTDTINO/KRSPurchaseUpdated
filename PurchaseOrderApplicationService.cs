using KRSPurchase.Domain;

namespace KRSPurchase.ApplicationService
{
    public class PurchaseOrderApplicationService
    {
        private readonly PurchaseOrderValidator _poValidator = new();
       
        private readonly IList<PurchaseOrder> _poList = new List<PurchaseOrder>()
        {
            new PurchaseOrder(new Supplier("PNPAY", "PicknPay", 1), new Item(new Good("KYBRD","Keyboard"), 5, 99.90m)),
            new PurchaseOrder(new Supplier("CNA01", "CnA", 1), new Item(new Good("PAPER","Paper"), 2, 45m)),
            new PurchaseOrder(new Supplier("SMSNG", "Samsung", 2), new Item(new Good("SPHNE","Samsung Phone"), 3, 10090m)),
        };

        public async Task<PurchaseOrder> FindByNumberAsync(int number)
        {
            return _poList.FirstOrDefault(po => po.Number == number)!;
        }
      
        public async Task<bool> AddAsync(PurchaseOrder purchaseOrder)
        {
                purchaseOrder.Number = _poList.Count + 1;

                var validationResult = _poValidator.Validate(purchaseOrder);
                if (!validationResult.IsValid) return false;
                _poList.Add(purchaseOrder);
                return true;
        }
        public async Task<bool> CancelAsync(int number)
        {
            var canceledPo = await FindByNumberAsync(number);
            if (canceledPo != null)
            {
                canceledPo.isCancelled = true;
                return true;
            }
            return false;
        }

        public async Task<IList<PurchaseOrder>> ListAsync()
        {
            return _poList;
        }

        public async Task<bool> AddItemAsync(PurchaseOrder purchaseOrder, List<Item> item)
        {
            item.ForEach(purchaseOrder.Add);
            return true;
        }
    }
}
