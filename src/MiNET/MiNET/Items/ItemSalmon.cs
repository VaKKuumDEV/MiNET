namespace MiNET.Items
{
	public class ItemSalmon : FoodItem
	{
		public ItemSalmon() : base("minecraft:salmon", 460, 0, 1, 0.6)
		{
		}

		public override Item GetSmelt()
		{
			return new ItemCookedSalmon();
		}
	}
}