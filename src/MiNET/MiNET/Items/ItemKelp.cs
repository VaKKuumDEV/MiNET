namespace MiNET.Items
{
	public class ItemKelp : Item
	{
		public ItemKelp() : base("minecraft:kelp", 335)
		{
		}

		public override Item GetSmelt()
		{
			return new ItemDriedKelp();
		}
	}
}