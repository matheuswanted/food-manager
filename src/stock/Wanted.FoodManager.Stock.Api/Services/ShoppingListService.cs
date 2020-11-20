using Grpc.Core;
using System.Linq;
using System.Threading.Tasks;
using Wanted.FoodManager.Stock.Api.Repositories;

namespace Wanted.FoodManager.Stock.Api
{

    public class ShoppingListService : ShoppingListGrpc.ShoppingListGrpcBase
    {
        private readonly ShoppingListRepository _repository;

        public ShoppingListService(ShoppingListRepository repository)
        {
            _repository = repository;
        }

        public override async Task<ShoppingListsReply> ListShoppingLists(ShoppingListsRequest request, ServerCallContext context)
        {
            var result = (await _repository.List(0, 10))
                .Select(s =>
                {
                    var shoppingList = new ShoppingListReply
                    {
                        Id = s.Id,
                        Name = s.Name
                    };

                    shoppingList.Items.Add(s.Items.Select(i => new ShoppingListItemReply
                    {
                        Name = i.Name,
                        Amount = i.Amount,
                        Unit = (Unit)(int)i.Unit
                    }));

                    return shoppingList;
                });

            var reply = new ShoppingListsReply();

            reply.Content.Add(result);

            return reply;
        }
    }
}
