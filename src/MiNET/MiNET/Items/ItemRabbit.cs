namespace MiNET.Items
{
	public class ItemRabbit : FoodItem
	{
		public ItemRabbit() : base("minecraft:rabbit", 411, 0, 1, 0.6)
		{
		}

		public override Item GetSmelt()
		{
			return new ItemCookedRabbit();
		}
	}
}