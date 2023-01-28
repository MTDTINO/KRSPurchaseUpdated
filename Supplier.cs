using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRSPurchase.Domain
{
    public class Supplier
    {
        public string Code { get; set; }
        public string Name { get;set; }
        public int LeadTime { get; set; }
    
        public Supplier(string code, string name ,int leadtime)
        {
            Code = code;
            Name = name;
            LeadTime = leadtime;
        }
    }
}
