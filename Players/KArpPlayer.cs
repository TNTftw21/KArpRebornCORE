using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using KArpRebornCORE.NPCs;
using KArpRebornCORE.NPCs.Components;

using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace KArpRebornCORE.Players
{
    public class KArpPlayer : ModPlayer
    {
        public int[] Stats { get; private set; }
        public int spentPoints => Stats[0] + Stats[1] + Stats[2];
        public int totalPoints => level - 1;
        public int experience { get; private set; }
        public int level { get; internal set; }
        public int baseEvasion = 0;
        public float evasionIncrease = 1.0f;
        public int totalEvasion => (int)(baseEvasion * evasionIncrease);
        public int baseAccuracy = 0;
        public float accuracyIncrease = 1.0f;
        public int totalAccuracy => (int)(baseAccuracy * accuracyIncrease);

        public event LevelUpEventHandler OnLevelUp;
        public delegate void LevelUpEventHandler(KArpPlayer krpgPlayer, Player player);

        public KArpPlayer()
        {
            Stats = new int[(int)PlayerStats._END];
            experience = 0;
            level = 1;
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (victim is NPC)
            {
                NPC npc = (NPC) victim;
                NPCComponentSystem npccs = npc.GetGlobalNPC<NPCComponentSystem>();
                if (npccs == null)
                {
                    KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have an NPCComponentSystem attached!", npc.FullName);
                    return;
                }
                NPCCombatTracker npcct = npccs.GetComponent<NPCCombatTracker>();
                if (npcct == null)
                    npcct = npccs.AddComponent(new NPCCombatTracker());
                
                npcct.AddPlayer(this.player);
                Networking.NPCCTSyncPacket.Write(npc.whoAmI, player.whoAmI);
            }
        }

        public override void PreUpdate()
        {
            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            baseEvasion = config.PlayerEvasionBase + (level * config.PlayerEvasionGrowth);
            baseAccuracy = config.PlayerAccuracyBase + (level * config.PlayerAccuracyGrowth);
            evasionIncrease += config.QuicknessEvasionPerPoint * Stats[(int)PlayerStats.Quickness];
            accuracyIncrease += config.QuicknessAccuracyPerPoint * Stats[(int)PlayerStats.Quickness];
        }

        public override void PostUpdateEquips()
        {
            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            // statLifeMax is the player's vanilla health. We'll use this value to increase the player's total life after mods,
            //  which essentially turns life crystals/fruit into a multiplier instead of flat addition.
            float lifeMultiplier = 1f + (player.statLifeMax - 100f) / 400f;
            // Figure out how much life, if any, has been added by other mods.
            int addedLife = player.statLifeMax2 - player.statLifeMax;
            player.statLifeMax2 += 115 + Stats[(int)PlayerStats.Resilience] * config.ResilienceHealthPerPoint + level * config.PlayerHealthGrowth - player.statLifeMax;
            // We're trying to treat health gained from leveling as base health. Increase it by life crystals/fruits, and *then* factor in other modifiers.
            player.statLifeMax2 = (int)Math.Round(player.statLifeMax2 * lifeMultiplier) + addedLife;
            float manaMultiplier = 1f + (player.statManaMax - 20f) / 180f;
            int addedMana = player.statManaMax2 - player.statManaMax;
            player.statManaMax2 += 19 + level + Stats[(int)PlayerStats.Wits] * config.WitsManaPerPoint - player.statManaMax;
            player.statManaMax2 = (int)Math.Round(player.statManaMax2 * manaMultiplier) + addedMana;
            player.statDefense += config.ResilienceDefensePerPoint * Stats[(int)PlayerStats.Resilience];

            player.allDamage *= 1f + config.PotencyDamagePerPoint * Stats[(int)PlayerStats.Potency];
            player.meleeSpeed += config.QuicknessAttackSpeedPerPoint * Stats[(int)PlayerStats.Quickness];
            player.moveSpeed += config.QuicknessMovementPerPoint * Stats[(int)PlayerStats.Quickness];
            player.jumpSpeedBoost += config.QuicknessMovementPerPoint * Stats[(int)PlayerStats.Quickness];
        }

        public override void ModifyHitNPC(Item item, NPC npc, ref int damage, ref float knockback, ref bool crit)
        {
            NPCComponentSystem npccs = npc.GetGlobalNPC<NPCComponentSystem>();
            if (npccs == null)
            {
                KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have an NPCComponentSystem attached!", npc.FullName);
                return;
            }

            NPCCombatTracker npcct = npccs.TryGetComponent<NPCCombatTracker>(() => new NPCCombatTracker());
            npcct.AddPlayer(this.player);
            Networking.NPCCTSyncPacket.Write(npc.whoAmI, player.whoAmI);

            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            if (config.DoEnemyEvasion) {

                NPCStatDef npcsd = npccs.TryGetComponent<NPCStatDef>(() => new NPCStatDef());

                if (NPCHelper.CalcDodge(totalAccuracy, npcsd.GetEvasion())) {
                    damage = 0;
                    knockback = 0;
                    crit = false;
                    CombatText.NewText(npc.getRect(), Color.Green, "Evaded!");
                    return;
                }
            }

            this.player.HealEffect((int)(config.PotencyLeechPerPoint * Stats[(int)PlayerStats.Potency] * damage));
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {

            NPCComponentSystem npccs = target.GetGlobalNPC<NPCComponentSystem>();
            if (npccs == null)
            {
                KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have an NPCComponentSystem attached!", target.FullName);
                return;
            }

            NPCCombatTracker npcct = npccs.TryGetComponent<NPCCombatTracker>(() => new NPCCombatTracker());
            npcct.AddPlayer(this.player);
            Networking.NPCCTSyncPacket.Write(target.whoAmI, player.whoAmI);

            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            if (config.DoEnemyEvasion) {

                NPCStatDef npcsd = npccs.TryGetComponent<NPCStatDef>(() => new NPCStatDef());

                if (NPCHelper.CalcDodge(totalAccuracy, npcsd.GetEvasion())) {
                    damage = 0;
                    knockback = 0;
                    crit = false;
                    CombatText.NewText(target.getRect(), Color.Green, "Evaded!");
                    return;
                }
            }

            this.player.HealEffect((int)(config.PotencyLeechPerPoint * Stats[(int)PlayerStats.Potency] * damage));
        }

        public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
        {
            KArpPlayer defenderPlayer = target.GetModPlayer<KArpPlayer>();
            if (defenderPlayer == null) {
                KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have a KArpPlayer attached!", target.name);
                return;
            }

            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            if (config.DoPlayerEvasion) {
                if (NPCHelper.CalcDodge(totalAccuracy, totalEvasion)) {
                    damage = 0;
                    crit = false;
                    CombatText.NewText(target.getRect(), Color.Green, "Evaded!");
                }
            }

            this.player.HealEffect((int)(config.PotencyLeechPerPoint * Stats[(int)PlayerStats.Potency] * damage));
        }

        public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
        {
            KArpPlayer defenderPlayer = target.GetModPlayer<KArpPlayer>();
            if (defenderPlayer == null) {
                KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have a KArpPlayer attached!", target.name);
                return;
            }

            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            if (config.DoPlayerEvasion) {
                if (NPCHelper.CalcDodge(totalAccuracy, defenderPlayer.totalEvasion)) {
                    damage = 0;
                    crit = false;
                    CombatText.NewText(target.getRect(), Color.Green, "Evaded!");
                }
            }

            this.player.HealEffect((int)(config.PotencyLeechPerPoint * Stats[(int)PlayerStats.Potency] * damage));
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (damage <= 0) {
                this.player.immune = true;
                this.player.immuneTime = 20;
                NetMessage.SendData(Terraria.ID.MessageID.Dodge, -1, -1, null, player.whoAmI, 1f);
                return false;
            }
            return true;
        }

        public void AddXp(int xp)
        {
            if (Main.gameMenu)
                return;
            if (xp <= 0)
                return;
            experience += xp;

            int xpToLevel = ExperienceToLevel();
            while (experience >= xpToLevel)
            {
                experience -= xpToLevel;
                LevelUp();
                xpToLevel = ExperienceToLevel();
            }

            CombatText.NewText(player.getRect(), new Microsoft.Xna.Framework.Color(127, 159, 255), $"{xp} XP");
        }

        public void LevelUp()
        {
            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            level += 1;
            baseEvasion += config.PlayerEvasionGrowth;
            baseAccuracy += config.PlayerAccuracyGrowth;
            Main.NewText($"Congratulations! You are now level {level}", 255, 223, 63);

            LevelUpEventHandler eventHandler = OnLevelUp;
            if (eventHandler != null)
            {
                eventHandler(this, this.player);
            }
        }

        public int ExperienceToLevel()
        {
            if (level < 5)
                return 80 + level * 20;
            if (level < 10)
                return level * 40;
            if (level < 163)
                return (int)(280 * Math.Pow(1.09, level - 5) + 3 * level);
            return (int)(2_000_000_000 - 288_500_000_000 / level);
        }

        public override TagCompound Save()
        {
            TagCompound tagCompound = new TagCompound
            {
                { "level", level },
                { "experience", experience },
                { "stats", Stats }
            };

            return tagCompound;
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("level"))
                this.level = tag.GetInt("level");
            if (tag.ContainsKey("experience"))
                this.experience = tag.GetInt("experience");
            if (tag.ContainsKey("stats"))
                this.Stats = tag.GetIntArray("stats");

            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            this.baseEvasion = config.PlayerEvasionBase + (config.PlayerEvasionGrowth * this.level);
            this.baseAccuracy = config.PlayerAccuracyBase + (config.PlayerAccuracyGrowth * this.level);
        }

        public override void clientClone(ModPlayer clientClone)
        {
            //Shouldn't strictly be necessary, but the fact that it's even possible to pass something that isn't KArpPlayer makes me use this.
            KArpPlayer clone = clientClone as KArpPlayer;
            if (clone == null)
                return;

            clone.level = this.level;
            clone.Stats = this.Stats;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            Networking.SyncPlayerPacket.Write(player.whoAmI, level, Stats);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            KArpPlayer clone = clientPlayer as KArpPlayer;
            if (clone == null)
                return;
            
            //If one has changed, it's highly likely the other has too
            if (clone.level != this.level || !clone.Stats.deepCompare(this.Stats)) {
                Networking.SyncPlayerPacket.Write(player.whoAmI, level, Stats);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Proxies.ClientProxy proxy = (KArpRebornCOREMain.Mod.proxy as Proxies.ClientProxy);
            if (proxy.characterScreenHotkey.JustPressed)
            {
                if (proxy._characterScreen.CurrentState == null)
                    proxy._characterScreen.SetState(proxy.characterScreen);
                else
                    proxy._characterScreen.SetState(null);
            }
        }
    }
}
