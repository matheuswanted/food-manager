using System.Collections.Generic;
using System.Linq;
using Wanted.FoodManager.Stock.Api;
using Xunit;

namespace Wanted.FoodManager.Stock.IT
{
    public class ShoppingListServiceIT : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;
        private ShoppingListGrpc.ShoppingListGrpcClient _client;

        public ShoppingListServiceIT(TestFixture fixture)
        {
            _fixture = fixture;
            _client = new ShoppingListGrpc.ShoppingListGrpcClient(_fixture.GrpcChannel);
        }

        [Fact]
        public async void ShouldCreateAShoppingList()
        {
            var request = new CreateShoppingListRequest
            {
                Name = "First list"
            };

            var actual = await _client.CreateShoppingListAsync(request);

            Assert.NotNull(actual);
            Assert.NotNull(actual.Id);
            Assert.Equal(24, actual.Id.Length);
            Assert.Equal(request.Name, actual.Name);
        }

        [Fact]
        public async void ShouldCreateAShoppingListAndPutItemsOnIt()
        {
            var shoppingListRequest = new CreateShoppingListRequest
            {
                Name = "First list"
            };

            var shoppingList = await _client.CreateShoppingListAsync(shoppingListRequest);

            Assert.NotNull(shoppingList);

            var potatoItem = new AddShoppingListItemRequest
            {
                ShoppingListId = shoppingList.Id,
                Name = "Potato",
                Amount = 1,
                Unit = Unit.Kg
            };

            var brocolliItem = new AddShoppingListItemRequest
            {
                ShoppingListId = shoppingList.Id,
                Name = "Potato",
                Amount = 1,
                Unit = Unit.Kg
            };
            var actualPotato = await _client.AddShoppingListItemAsync(potatoItem);
            var actualBrocolli = await _client.AddShoppingListItemAsync(brocolliItem);

            var actualList = await _client.FindShoppingListAsync(new FindListsRequest { Id = shoppingList.Id });

            Assert.NotNull(actualPotato);
            Assert.NotNull(actualBrocolli);
            Assert.NotNull(actualList);
            Assert.Equal(shoppingList.Id, actualList.Id);
            var actualItems = actualList.Items.ToList();
            Assert.Equal(new List<ShoppingListItemReply> { actualPotato, actualBrocolli }, actualItems);
        }
    }
}
