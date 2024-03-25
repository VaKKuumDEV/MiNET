using MiNET.Sounds;
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
							if (blockk is RedstoneLamp) { level.SetBlock(new LitRedstoneLamp { Coordinates = new BlockCoordinates(cord) }); }
							if (blockk is WoodenDoor) { level.SetBlock(new WoodenDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = true }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
							if (blockk is SpruceDoor) { level.SetBlock(new SpruceDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = true }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
							if (blockk is BirchDoor) { level.SetBlock(new BirchDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = true }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
							if (blockk is JungleDoor) { level.SetBlock(new JungleDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = true }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
							if (blockk is AcaciaDoor) { level.SetBlock(new AcaciaDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = true }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
							if (blockk is DarkOakDoor) { level.SetBlock(new DarkOakDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = true }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
							if (blockk is IronDoor) { level.SetBlock(new IronDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = true }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
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
						if (blockk is LitRedstoneLamp) { level.SetBlock(new RedstoneLamp { Coordinates = new BlockCoordinates(cord) });}
						if (blockk is WoodenDoor) { level.SetBlock(new WoodenDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = false }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
						if (blockk is SpruceDoor) { level.SetBlock(new SpruceDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = false }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
						if (blockk is BirchDoor) { level.SetBlock(new BirchDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = false }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
						if (blockk is JungleDoor) { level.SetBlock(new JungleDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = false }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
						if (blockk is AcaciaDoor) { level.SetBlock(new AcaciaDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = false }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
						if (blockk is DarkOakDoor) { level.SetBlock(new DarkOakDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = false }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
						if (blockk is IronDoor) { level.SetBlock(new IronDoor { Direction = blockk.GetDirection(), Coordinates = new BlockCoordinates(cord), OpenBit = false }); var sound = new Sound((short) LevelEventType.SoundOpenDoor, Coordinates); sound.Spawn(level);}
					}
				}
			}
			level.ScheduleBlockTick(this, 20);
		}

	}
}
