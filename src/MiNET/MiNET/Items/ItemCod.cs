namespace MiNET.Items
{
	public class ItemCod : FoodItem
	{
		public ItemCod() : base("minecraft:cod", 349, 0, 1, 0.6)
		{
		}

		public override Item GetSmelt()
		{
			return new ItemCookedCod();
		}
	}
}