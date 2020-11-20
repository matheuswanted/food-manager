using System.Collections.Generic;
using System.Linq;

namespace Wanted.FoodManager.Stock.Domain
{
    public class ShoppingList : Entity
    {
        public string Name { get; set; }
        public List<ShoppingListItem> Items { get; set; }
    }
}
