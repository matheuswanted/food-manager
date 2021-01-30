using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wanted.FoodManager.Stock.Api.Repositories;
using Wanted.FoodManager.Stock.Domain;

namespace Wanted.FoodManager.Stock.Api
{

    public class ShoppingListService : ShoppingListGrpc.ShoppingListGrpcBase
    {
        private readonly ShoppingListRepository _repository;

        public ShoppingListService(ShoppingListRepository repository)
        {
            _repository = repository;
        }

        public override async Task<ShoppingListItemReply> AddShoppingListItem(AddShoppingListItemRequest request, ServerCallContext context)
        {
            var shoppingList = await _repository.Find(request.ShoppingListId);
            if (shoppingList == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Shopping list not found"));
            }

            shoppingList.Items.Add(new ShoppingListItem
            {
                Name = request.Name,
                Amount = request.Amount,
                Unit = (Domain.Unit)(int)request.Unit
            });

            await _repository.Save(shoppingList);

            return AsReply(shoppingList.Items.LastOrDefault());
        }

        public override async Task<ShoppingListReply> CreateShoppingList(CreateShoppingListRequest request, ServerCallContext context)
        {
            var shoppingList = new ShoppingList()
            {
                Name = request.Name,
                Items = new List<ShoppingListItem>()
            };

            return AsReply(await _repository.Save(shoppingList));
        }

        public override async Task<ShoppingListsReply> ListShoppingLists(ShoppingListsRequest request, ServerCallContext context)
        {
            var result = (await _repository.List(0, 10))
                .Select(AsReply);

            var reply = new ShoppingListsReply();

            reply.Content.Add(result);

            return reply;
        }

        public override async Task<ShoppingListReply> FindShoppingList(FindListsRequest request, ServerCallContext context)
        {
            var found = await _repository.Find(request.Id) ?? throw new RpcException(new Status(StatusCode.NotFound, "Shopping list not found"));
            return AsReply(found);
        }

        private ShoppingListReply AsReply(ShoppingList shoppingList)
        {
            var reply = new ShoppingListReply
            {
                Id = shoppingList.Id,
                Name = shoppingList.Name
            };

            foreach (var item in shoppingList.Items ?? new List<ShoppingListItem>())
            {
                reply.Items.Add(AsReply(item));
            }

            return reply;
        }

        private ShoppingListItemReply AsReply(ShoppingListItem item)
        {
            return new ShoppingListItemReply
            {
                Name = item.Name,
                Amount = item.Amount,
                Unit = (Unit)(int)item.Unit
            };
        }
    }
}
