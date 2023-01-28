using KRSPurchase.ApplicationService;
using KRSPurchase.Domain;
using System.Runtime.InteropServices;

namespace KRSPurchase.Test
{
    public class ManageGoods
    {
        private readonly GoodsApplicationService _service = new();
        private readonly GoodValidator _validator = new();

        [Fact]
        public void ShouldCreateANewGood()
        {
            // given a code "ABCDE" and name "Alpha"
            const string code = "ABCDE";
            const string name = "Alpha";

            //when we create a good
            var good = new Good(code, name);

            //then the good should exist with code "ABCDE" and name "Alpha"
            Assert.NotNull(good);
            Assert.Equal(code, good.Code);
            Assert.Equal(name, good.Name);
        }

        [Fact]
        public void ShouldValidateGood()
        {
            //give a code "MKR12" and name "Marker"
            var good = new Good("MKR12", "Marker");

            //when we validate the good 
            var validationResult = _validator.Validate(good);

            //then the good should be valid
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ShouldFailToValidateGoods()
        {
            //give a code "MKR12" and name of nothing
            var good = new Good("MKR12", "");


            //when we validate the good 
            var validationResult = _validator.Validate(good);

            //then the good should be invalid
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task ShouldGetAListOfGoods()
        {
            //given an application service containing a good with a code "BK001"
            //when we get a list of goods

            var goods = await _service.ListAsync();
            //then we should have a list with the existing goods
            Assert.Contains(goods, gd => gd.Code == "MOU01");
            Assert.True(goods.Count() > 1);
        }

        [Fact]
        public async Task ShouldFindGoodByCode()
        {
            // given a good with the name "PAPER" and code "Paper"
            const string code = "PAPER";

            //and an goods application service containing a good with that code

            //when we find the good
            var good = await _service.FindByCodeAsync(code);

            //then the good should exist with that code
            Assert.NotNull(good);
            Assert.Equal(code, good.Code);
        }

        [Fact]
        public async Task ShouldAddGood()
        {
            //given a code "PENCL" and name of "Pencil"
            var good = new Good("PENCL", "Pencil");

            //when we add the the good
            var goodAdded = await _service.AddAsync(good);

            //then the good should be added successfully
            Assert.True(goodAdded);
        }

        [Fact]
        public async Task ShouldDeleteGoodByCode()
        {
            //given a code "TABLE" 
            //and a good application service containing a good with that code
            const string code = "TABLE";
            const string name = "Table";

            var goodAdded = await _service.AddAsync(new Good(code, name));
            Assert.True(goodAdded);

            //when we delete the good 
            var goodDeleted = await _service.DeleteAsync(code);

            //then the good should be deleted
            Assert.True(goodDeleted);
            var deletedGood = await _service.FindByCodeAsync(code);
            Assert.Null(deletedGood);
        }
        [Fact]
        public async Task ShouldEditGood()
        {
            //given a code "TABLE" and name "Table"
            const string code = "KYBRD";
            const string name = "Keyboard";

            //when we find the good
            var existingGood = await _service.FindByCodeAsync(code);
            existingGood.Name = name;

            var goodEdited = await _service.EditAsync(existingGood);
            
            //then the good should be edited
            var editedGood = await _service.FindByCodeAsync(code);
            Assert.True(goodEdited);
            Assert.Equal(name, editedGood.Name);
        }

        [Fact]

        public async Task ShouldCheckForDuplicateGood()
        {
            ////given a code "PAPER" 
            //and a good application service containing a good with that code
            const string code = "PAPER";
            const string name = "Paper";
            var good = new Good(code, name);

            //when try add the good
            var addGood = await _service.AddAsync(good);

            //then the good should not be added
            Assert.False(addGood);
        }
    }
}