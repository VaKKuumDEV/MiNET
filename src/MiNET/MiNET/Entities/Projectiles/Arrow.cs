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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Numerics;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Entities.Passive;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class Arrow : Projectile
	{
		public byte Effect { get; set; } = 0;
		public int PickupDelay = 10;
		public Arrow(Player shooter, Level level, int damage = 2, bool isCritical = false) : base(shooter, EntityType.ShotArrow, level, damage, isCritical)
		{
			Width = 0.15;
			Length = 0.15;
			Height = 0.15;

			Gravity = 0.05;
			Drag = 0.01;

			//OK: Drag = 0.0083;

			HealthManager.IsInvulnerable = true;
			Ttl = 1200;
			DespawnOnImpact = false;
		}

		protected override void OnHitBlock(Block blockCollided)
		{
			IsCritical = false;
			BroadcastSetEntityData();

			McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
			entityEvent.runtimeEntityId = EntityId;
			entityEvent.eventId = 39;
			entityEvent.data = 14;
			Level.RelayBroadcast(entityEvent);
			Level.BroadcastSound(blockCollided.Coordinates, LevelSoundEventType.BowHit);
		}

		protected override void OnHitEntity(Entity entityCollided)
		{
			IsCritical = false;
			BroadcastSetEntityData();
			Effect effect = getEffect(Effect);
			effect.Duration = effect.Duration / 8;
			if (entityCollided is Player player && effect != null)
			{
				player.SetEffect(effect);
			}
			else if (entityCollided is PassiveMob mob) //todo
			{
			}
		}

		public override void OnTick(Entity[] entities)
		{
			base.OnTick(entities);

			if (PickupDelay > 0)
			{
				PickupDelay--;
			}
			else if(Velocity == Vector3.Zero)
			{
				var bbox = GetBoundingBox();
				var players = Level.GetSpawnedPlayers();
				foreach (var player in players)
				{
					if (player.GameMode != GameMode.Spectator && bbox.Intersects(player.GetBoundingBox() + 1))
					{
						if (player.Inventory.SetFirstEmptySlot(ItemFactory.GetItem("minecraft:arrow", Effect), true))
						{
							var takeItemEntity = McpeTakeItemEntity.CreateObject();
							takeItemEntity.runtimeEntityId = EntityId;
							takeItemEntity.target = player.EntityId;
							Level.RelayBroadcast(takeItemEntity);
						}
						DespawnEntity();
					}
				}
			}
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = base.GetMetadata();
			metadata[(int) MetadataFlags.CustomDisplay] = new MetadataByte(Effect);
			return metadata;
		}

		private static Effect getEffect(int Metadata)
		{
			Effect e = null;
			switch (Metadata)
			{
				case 6:
					e = new NightVision
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 7:
					e = new NightVision
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 8:
					e = new Invisibility
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 9:
					e = new Invisibility
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 10:
					e = new JumpBoost
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 11:
					e = new JumpBoost
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 12:
					e = new JumpBoost
					{
						Duration = 1800,
						Level = 1
					};
					break;
				case 13:
					e = new FireResistance
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 14:
					e = new FireResistance
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 15:
					e = new Speed
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 16:
					e = new Speed
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 17:
					e = new Speed
					{
						Duration = 1800,
						Level = 1
					};
					break;
				case 18:
					e = new Slowness
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 19:
					e = new Slowness
					{
						Duration = 4800,
						Level = 0
					};
					break;
				case 20:
					e = new WaterBreathing
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 21:
					e = new WaterBreathing
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 22:
					e = new InstantHealth
					{
						Duration = 0,
						Level = 0
					};
					break;
				case 23:
					e = new InstantHealth
					{
						Duration = 0,
						Level = 1
					};
					break;
				case 24:
					e = new InstantDamage
					{
						Duration = 0,
						Level = 0
					};
					break;
				case 25:
					e = new InstantDamage
					{
						Duration = 0,
						Level = 1
					};
					break;
				case 26:
					e = new Poison
					{
						Duration = 900,
						Level = 0
					};
					break;
				case 27:
					e = new Poison
					{
						Duration = 2400,
						Level = 0
					};
					break;
				case 28:
					e = new Poison
					{
						Duration = 440,
						Level = 1
					};
					break;
				case 29:
					e = new Regeneration
					{
						Duration = 900,
						Level = 0
					};
					break;
				case 30:
					e = new Regeneration
					{
						Duration = 2400,
						Level = 0
					};
					break;
				case 31:
					e = new Regeneration
					{
						Duration = 440,
						Level = 1
					};
					break;
				case 32:
					e = new Strength
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 33:
					e = new Strength
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 34:
					e = new Strength
					{
						Duration = 1800,
						Level = 1
					};
					break;
				case 35:
					e = new Weakness
					{
						Duration = 1800,
						Level = 0
					};
					break;
				case 36:
					e = new Weakness
					{
						Duration = 4800,
						Level = 0
					};
					break;
				case 37:
					e = new Wither
					{
						Duration = 800,
						Level = 0
					};
					break;
				/*case 38:           these effects doesn't exist but will.
					e = new TurtleMaster
					{
						Duration = 400,
						Level = 0
					};
					break;
				case 39:
					e = new TurtleMaster
					{
						Duration = 800,
						Level = 1
					};
					break;
				case 40:
					e = new TurtleMaster
					{
						Duration = 400,
						Level = 2
					};
					break;
				case 41:
					e = new SlowFalling
					{
						Duration = 2600,
						Level = 0
					};
					break;
				case 42:
					e = new SlowFalling
					{
						Duration = 4800,
						Level = 0
					};
					break;*/
			}

			if (e != null)
			{
				return e;
			}
			return null;
		}
	}
}