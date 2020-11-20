using System.Collections.Generic;

namespace Wanted.FoodManager.Stock.Domain
{
    public class ShoppingList : Entity
    {
        public string Name { get; private set; }
        public List<ShoppingListItem> Items { get; set; }
    }
}
