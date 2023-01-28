namespace KRSPurchase.Domain
{
    public class Item
    {
        public Good Good { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Item(Good good, int quantity, decimal price)
        {
            Good = good;    
            Quantity = quantity;
            Price = price;
        }
    }
}
