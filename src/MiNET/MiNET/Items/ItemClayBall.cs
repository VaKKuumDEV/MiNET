namespace MiNET.Items
{
	public class ItemClayBall : Item
	{
		public ItemClayBall() : base("minecraft:clay_ball", 337)
		{
		}

		public override Item GetSmelt()
		{
			return new ItemBrick();
		}
	}
}