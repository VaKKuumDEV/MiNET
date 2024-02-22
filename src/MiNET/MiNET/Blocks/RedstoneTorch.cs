#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class RedstoneTorch : Block
	{
		public RedstoneTorch() : base(76)
		{
			LightLevel = 7;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{

			if (face == BlockFace.Down)
				return true;

			switch (face)
			{
				case BlockFace.Up:
					TorchFacingDirection = "top";
					break;
				case BlockFace.North:
					TorchFacingDirection = "north";
					break;
				case BlockFace.South:
					TorchFacingDirection = "south";
					break;
				case BlockFace.West:
					TorchFacingDirection = "west";
					break;
				case BlockFace.East:
					TorchFacingDirection = "east";
					break;
			}

			return false;
		}

		public override void BreakBlock(Level level, BlockFace face, bool silent = false)
		{
			level.CancelBlockTick(this);
			BlockCoordinates cord = this.Coordinates.BlockNorth();
			for (int i = 0; i < 5; i++)
			{
				if (i == 0) { cord = this.Coordinates.BlockNorth(); }
				if (i == 1) { cord = this.Coordinates.BlockSouth(); }
				if (i == 2) { cord = this.Coordinates.BlockWest(); }
				if (i == 3) { cord = this.Coordinates.BlockEast(); }
				if (i == 4) { cord = this.Coordinates.BlockUp(); }
				var blockk = level.GetBlock(cord);
				if (blockk is LitRedstoneLamp)
				{
					level.SetBlock(new RedstoneLamp { Coordinates = new BlockCoordinates(cord) });
				}
			}
			base.BreakBlock(level, face);
		}

		public override void BlockAdded(Level level)
		{
			if (level.RedstoneEnabled) { level.ScheduleBlockTick(this, 10); }
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (isRandom) { return; }
			BlockCoordinates cord = this.Coordinates.BlockNorth();
			for (int i = 0; i < 5; i++)
			{
				if (i == 0) { cord = this.Coordinates.BlockNorth(); }
				if (i == 1) { cord = this.Coordinates.BlockSouth(); }
				if (i == 2) { cord = this.Coordinates.BlockWest(); }
				if (i == 3) { cord = this.Coordinates.BlockEast(); }
				if (i == 4) { cord = this.Coordinates.BlockUp(); }
				var blockk = level.GetBlock(cord);
				if (blockk is RedstoneLamp)
				{
					level.SetBlock(new LitRedstoneLamp { Coordinates = new BlockCoordinates(cord) });
				}
			}
			level.ScheduleBlockTick(this, 10);
		}
	}
}