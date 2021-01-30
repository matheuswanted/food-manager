using MongoDB.Driver;
using Wanted.FoodManager.Stock.Domain;

namespace Wanted.FoodManager.Stock.Api.Repositories
{
    public class ShoppingListRepository : GenericRepository<ShoppingList>
    {
        public ShoppingListRepository(IMongoCollection<ShoppingList> collection) : base(collection)
        {
        }
    }
}
