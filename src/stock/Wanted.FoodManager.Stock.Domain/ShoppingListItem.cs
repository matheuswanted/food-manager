namespace Wanted.FoodManager.Stock.Domain
{
    public class ShoppingListItem
    {
        public string Name { get; private set; }
        public double Amount { get; private set; }
        public Unit Unit { get; private set; }
    }
}
