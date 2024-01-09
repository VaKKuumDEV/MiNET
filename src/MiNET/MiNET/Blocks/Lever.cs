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

using System;
using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Lever : Block
	{
		public static string RedstoneSignalDirection { get; set; } = "north";
		private static readonly ILog Log = LogManager.GetLogger(typeof(Lever));
		public Lever() : base(69)
		{
			IsTransparent = true;
			IsSolid = false;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var FacingDirection = ItemBlock.GetFacingDirectionFromEntity(player);
			Log.Debug(FacingDirection);
			if (face == BlockFace.Down)
			{
				LeverDirection = FacingDirection switch
				{
					5 or 4 => "down_east_west",
					2 or 3 => "down_north_south",
					_ => throw new ArgumentOutOfRangeException()
				};
			}
			else if (face == BlockFace.Up)
			{
				LeverDirection = FacingDirection switch
				{
					5 or 4 => "up_east_west",
					2 or 3 => "up_north_south",
					_ => throw new ArgumentOutOfRangeException()
				};
			}
			else
			{
				LeverDirection = FacingDirection switch
				{
					5 => "east",
					3 => "south",
					4 => "west",
					2 => "north",
					_ => throw new ArgumentOutOfRangeException()
				};
			}

			RedstoneSignalDirection = face switch
			{
				BlockFace.North => "south",
				BlockFace.South => "north",
				BlockFace.West => "east",
				BlockFace.East => "west",
				BlockFace.Up => "down",
				BlockFace.Down => "up",
				_ => throw new ArgumentOutOfRangeException()
			};

			return false;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			BlockCoordinates cord = blockCoordinates.BlockNorth();
			if (RedstoneSignalDirection == "north")
			{
				cord = blockCoordinates.BlockNorth();
			}
			else if (RedstoneSignalDirection == "south")
			{
				cord = blockCoordinates.BlockSouth();
			}
			else if (RedstoneSignalDirection == "west")
			{
				cord = blockCoordinates.BlockWest();
			}
			else if (RedstoneSignalDirection == "east")
			{
				cord = blockCoordinates.BlockEast();
			}
			else if (RedstoneSignalDirection == "up")
			{
				cord = blockCoordinates.BlockUp();
			}
			else if (RedstoneSignalDirection == "down")
			{
				cord = blockCoordinates.BlockDown();
			}
			if (!OpenBit)
			{
				var blockk = world.GetBlock(cord);
				if (blockk is RedstoneLamp)
				{
					world.SetBlock(new LitRedstoneLamp { Coordinates = new BlockCoordinates(cord) });
				}
			}
			else
			{
				var blockk = world.GetBlock(cord);
				if (blockk is RedstoneLamp)
				{
					world.SetBlock(new RedstoneLamp { Coordinates = new BlockCoordinates(cord) });
				}
			}
			OpenBit = !OpenBit;
			world.SetBlock(this);
			return true;
		}

	}
}