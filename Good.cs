namespace KRSPurchase.Domain
{
    public class Good
    {
        public string Code { get; }
        public string Name { get; set; }

        public Good(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}