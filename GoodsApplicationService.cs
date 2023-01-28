using KRSPurchase.Domain;
using System.Xml.Linq;

namespace KRSPurchase.ApplicationService
{
    public class GoodsApplicationService
    {
        private readonly GoodValidator _validator = new();


        private IList<Good> _goodList = new List<Good>()
        {
            new Good("MOU01","Mouse"),
            new Good("CABLE","Cable"),
            new Good("USBDR","Usb Drive"),
            new Good ("PAPER","Paper"),
            new Good("KYBRD", "Keyboord"),
        };

        public async Task<IList<Good>> ListAsync()
        {
            return _goodList;
        }

        public async Task<Good> FindByCodeAsync(string code)
        {
            return _goodList.FirstOrDefault(g => g.Code == code)!;
        } 

        public async Task<bool> AddAsync(Good good)
        {
            var validationResult = _validator.Validate(good);
            if (await CheckDuplicateAysnc(good.Code)) return false;
            if (!validationResult.IsValid) return false;

            _goodList.Add(good);
            return true;
        }

        public async Task<bool> DeleteAsync(string code)
        {
            var good = await FindByCodeAsync(code);
            if (good == null) return false;

            _goodList.Remove(good);
            return true;
        }
        
        public async Task<bool> CheckDuplicateAysnc(string code) 
        {
            var goodDuplicate = await FindByCodeAsync(code);

            return goodDuplicate != null;

        }

        public async Task<bool> EditAsync(Good existingGood)
        {
            var good = await FindByCodeAsync(existingGood.Code);
            var validationResult = _validator.Validate(good);

            if (validationResult.IsValid) 
            {
                _goodList = _goodList.Select(g => g.Code == existingGood.Code ? existingGood : g).ToList();
                return true;
            }
            return false;
        }
    }
}