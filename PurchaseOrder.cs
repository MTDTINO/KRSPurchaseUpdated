namespace KRSPurchase.Domain
{
    public class PurchaseOrder
    {
        public int Number { get; set; } = 1;
        public bool isCancelled { get; set; }
        public DateTime OrderDate { get; set; }

        public Supplier Supplier { get; set; }

        public DateTime EstimatedDeliveryDate => OrderDate.AddDays(Supplier.LeadTime);

        private IList<Item> _items = new List<Item>();
        public IEnumerable<Item> Items => _items;

        public PurchaseOrder(Supplier supplier, Item item)
        {
            OrderDate = DateTime.Now;
            Supplier = supplier;
            _items.Add(item);
        }

        public void Add(Item item)
        {
            //check if a item with a same good code exist
            //then if doesnt exist add it to the items list
            //if it does exist then increase the quantity of te found item by the quantity of the item coming from parameter
            var itemInTheList = _items.FirstOrDefault(i => i.Good.Code == item.Good.Code);

            if (itemInTheList == null)
                _items = _items.Append(item).ToList();

            else   _items = _items.Select(i => i.Good.Code == item.Good.Code
                ? new Item(itemInTheList.Good, itemInTheList.Quantity += item.Quantity, itemInTheList.Price)
                : i).ToList();

            //_items.Add(item);
        }

    }
}
