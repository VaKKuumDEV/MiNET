namespace MiNET.Items
{
	public class ItemChorusFruit : Item
	{
		public ItemChorusFruit() : base("minecraft:chorus_fruit", 432)
		{
		}

		public override Item GetSmelt()
		{
			return new ItemPoppedChorusFruit();
		}
	}
}