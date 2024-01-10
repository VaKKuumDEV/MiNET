using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class PressurePlateBase : Block
	{
		public virtual int RedstoneSignal { get; set; }

		protected PressurePlateBase(int id) : base(id)
		{
			IsTransparent = true;
			IsSolid = false;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override void BlockAdded(Level level)
		{
			if (level.RedstoneEnabled) { level.ScheduleBlockTick(this, 20); }
		}

		public override void BreakBlock(Level level, BlockFace face, bool silent = false)
		{
			level.CancelBlockTick(this);
			base.BreakBlock(level, face);
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (isRandom) { return; }
			BlockCoordinates cord = Coordinates.BlockDown();
			var entities = level.GetSpawnedPlayers();
			foreach (var entity in entities)
			{
				if ((int) entity.KnownPosition.X == Coordinates.X && (int) entity.KnownPosition.Y == Coordinates.Y && (int) entity.KnownPosition.Z == Coordinates.Z)
				{
					if (RedstoneSignal == 0)
					{
						RedstoneSignal = 15;
						level.SetBlock(this);
						level.BroadcastSound(Coordinates, LevelSoundEventType.PressurePlateOn);
						for (int i = 0; i < 5; i++)
						{
							if (i == 0) { cord = this.Coordinates.BlockNorth(); }
							if (i == 1) { cord = this.Coordinates.BlockSouth(); }
							if (i == 2) { cord = this.Coordinates.BlockWest(); }
							if (i == 3) { cord = this.Coordinates.BlockEast(); }
							if (i == 4) { cord = this.Coordinates.BlockDown(); }
							var blockk = level.GetBlock(cord);
							if (blockk is RedstoneLamp)
							{
								level.SetBlock(new LitRedstoneLamp { Coordinates = new BlockCoordinates(cord) });
							}
						}
					}
				}
				else if (RedstoneSignal > 0)
				{
					RedstoneSignal = 0;
					level.SetBlock(this);
					level.BroadcastSound(Coordinates, LevelSoundEventType.PressurePlateOff);
					for (int i = 0; i < 5; i++)
					{
						if (i == 0) { cord = this.Coordinates.BlockNorth(); }
						if (i == 1) { cord = this.Coordinates.BlockSouth(); }
						if (i == 2) { cord = this.Coordinates.BlockWest(); }
						if (i == 3) { cord = this.Coordinates.BlockEast(); }
						if (i == 4) { cord = this.Coordinates.BlockDown(); }
						var blockk = level.GetBlock(cord);
						if (blockk is LitRedstoneLamp)
						{
							level.SetBlock(new RedstoneLamp { Coordinates = new BlockCoordinates(cord) });
						}
					}
				}
			}
			level.ScheduleBlockTick(this, 20);
		}

	}
}
