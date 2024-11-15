using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using MiNET.Effects;
using MiNET.Particles;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace MiNET.Entities.Projectiles
{
	public class SplashPotion : Projectile
	{
		public short Metadata { get; set; }
		public SplashPotion(Player shooter, Level level, short metadata) : base(shooter, EntityType.ThrownSpashPotion, level, 0)
		{
			Width = 0.25;
			Length = 0.25;
			Height = 0.25;

			Gravity = 0.15;
			Drag = 0.25;

			Damage = -1;
			HealthManager.IsInvulnerable = true;
			DespawnOnImpact = true;
			BroadcastMovement = true;
			Metadata = metadata;
		}

		public override void DespawnEntity()
		{
			var effects = GetEffects();
			var particle = new SplashPotionParticle(Level, KnownPosition, effects.Count == 0 ? Color.FromArgb(0x38, 0x5d, 0xc6) : effects[0].ParticleColor);
			particle.Spawn();
			Level.BroadcastSound(KnownPosition, LevelSoundEventType.Glass);

			var playersInArea = new List<Player>();

			foreach (var player in Level.Players.Values)
			{
				var box = player.GetBoundingBox() + 4.00f;
				if (box.Intersects(GetBoundingBox()))
				{
					playersInArea.Add(player);
				}
			}
			if(playersInArea.Count <= 0)
			{
				base.DespawnEntity();
				return;
			}
			ApplyPotionEffects(playersInArea, effects);
			base.DespawnEntity();
		}

		private void ApplyPotionEffects(List<Player> players, List<Effect> effects)
		{
			if (effects == null) return;
			foreach (var player in players)
			{
				foreach (var effect in effects)
				{
					float distance = Vector3.Distance(player.KnownPosition, KnownPosition);
					float multiplier = (1 - (distance / 4));

					effect.Particles = true;
					effect.Duration = (int)(effect.Duration * multiplier);
					player.SetEffect(effect);
				}
			}
		}

		public override MetadataDictionary GetMetadata()
		{
			return new MetadataDictionary
			{
				[(int) MetadataFlags.AuxValueData] = new MetadataShort(Metadata),
			};
		}

		private List<Effect> GetEffects()
		{
			List<Effect> effect = new List<Effect>();
			switch (Metadata)
			{
				case 5: // Splash Potion of Night Vision (2:15)
					effect.Add(new NightVision { Duration = 3000 });
					break;
				case 6: // Splash Potion of Night Vision (6:00)
					effect.Add(new NightVision { Duration = 7200 });
					break;
				case 7: // Splash Potion of Invisibility (2:15)
					effect.Add(new Invisibility { Duration = 3000 });
					break;
				case 8: // Splash Potion of Invisibility (6:00)
					effect.Add(new Invisibility { Duration = 7200 });
					break;
				case 9: // Splash Potion of Leaping (Jump Boost 1, 2:15)
					effect.Add(new JumpBoost { Duration = 3000, Level = 1 });
					break;
				case 10: // Splash Potion of Leaping (Jump Boost 1, 6:00)
					effect.Add(new JumpBoost { Duration = 7200, Level = 1 });
					break;
				case 11: // Splash Potion of Leaping (Jump Boost 2, 1:07)
					effect.Add(new JumpBoost { Duration = 1480, Level = 2 });
					break;
				case 12: // Splash Potion of Fire Resistance (2:15)
					effect.Add(new FireResistance { Duration = 3000 });
					break;
				case 13: // Splash Potion of Fire Resistance (6:00)
					effect.Add(new FireResistance { Duration = 7200 });
					break;
				case 14: // Splash Potion of Swiftness (Speed 1, 2:15)
					effect.Add(new Speed { Duration = 3000, Level = 1 });
					break;
				case 15: // Splash Potion of Swiftness (Speed 1, 6:00)
					effect.Add(new Speed { Duration = 7200, Level = 1 });
					break;
				case 16: // Splash Potion of Swiftness (Speed 2, 1:07)
					effect.Add(new Speed { Duration = 1480, Level = 2 });
					break;
				case 17: // Splash Potion of Slowness (1:07)
					effect.Add(new Slowness { Duration = 1480 });
					break;
				case 18: // Splash Potion of Slowness (3:00)
					effect.Add(new Slowness { Duration = 3600 });
					break;
				case 19: // Splash Potion of Water Breathing (2:15)
					effect.Add(new WaterBreathing { Duration = 3000 });
					break;
				case 20: // Splash Potion of Water Breathing (6:00)
					effect.Add(new WaterBreathing { Duration = 7200 });
					break;
				case 21: // Splash Potion of Healing
					effect.Add(new InstantHealth { Level = 1 });
					break;
				case 22: // Splash Potion of healing 2
					effect.Add(new InstantHealth { Level = 2 });
					break;
				case 23: // Splash Potion of Harming
					effect.Add(new InstantDamage { Level = 1 });
					break;
				case 24: // Splash Potion of Harming 2
					effect.Add(new InstantDamage { Level = 2 });
					break;
				case 25: // Splash Potion of Poison (Poison 1, 0:33)
					effect.Add(new Poison { Duration = 1320, Level = 1 });
					break;
				case 26: // Splash Potion of Poison (Poison 1, 1:30)
					effect.Add(new Poison { Duration = 2400, Level = 1 });
					break;
				case 27: // Splash Potion of Poison (Poison 2, 0:16)
					effect.Add(new Poison { Duration = 640, Level = 2 });
					break;
				case 28: // Splash Potion of Regeneration (Regen 1, 0:33)
					effect.Add(new Regeneration { Duration = 1320, Level = 1 });
					break;
				case 29: // Splash Potion of Regeneration (Regen 1, 1:30)
					effect.Add(new Regeneration { Duration = 2400, Level = 1 });
					break;
				case 30: // Splash Potion of Regeneration (Regen 2, 0:16)
					effect.Add(new Regeneration { Duration = 640, Level = 2 });
					break;
				case 31: // Splash Potion of Strength (Strength 1, 2:15)
					effect.Add(new Strength { Duration = 3000, Level = 1 });
					break;
				case 32: // Splash Potion of Strength (Strength 1, 6:00)
					effect.Add(new Strength { Duration = 7200, Level = 1 });
					break;
				case 33: // Splash Potion of Strength (Strength 2, 1:07)
					effect.Add(new Strength { Duration = 1480, Level = 2 });
					break;
				case 34: // Splash Potion of Weakness (1:07)
					effect.Add(new Weakness { Duration = 1480 });
					break;
				case 35: // Splash Potion of Weakness (3:00)
					effect.Add(new Weakness { Duration = 3600 });
					break;
				case 36: // Splash Potion of Decay (Wither 2, 0:30)
					effect.Add(new Wither { Duration = 1200, Level = 2 });
					break;
				case 37: // Splash Potion of the Turtle Master (Slowness 4 & Resistance 3, 0:15)
					effect.Add(new Slowness { Duration = 300, Level = 4 });
					effect.Add(new Resistance { Duration = 300, Level = 3 });
					break;
				case 38: // Splash Potion of the Turtle Master (Slowness 4 & Resistance 3, 0:30)
					effect.Add(new Slowness { Duration = 600, Level = 4 });
					effect.Add(new Resistance { Duration = 600, Level = 3 });
					break;
				case 39: // Splash Potion of the Turtle Master (Slowness 6 & Resistance 4, 0:15)
					effect.Add(new Slowness { Duration = 300, Level = 6 });
					effect.Add(new Resistance { Duration = 300, Level = 4 });
					break;
				case 40: // Splash Potion of Slow Falling (1:07)
					effect.Add(new SlowFalling { Duration = 1480 });
					break;
				case 41: // Splash Potion of Slow Falling (3:00)
					effect.Add(new SlowFalling { Duration = 3600 });
					break;
			}
			return effect;
		}
	}
}
