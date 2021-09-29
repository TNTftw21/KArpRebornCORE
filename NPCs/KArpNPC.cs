using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using KArpRebornCORE.Players;
using KArpRebornCORE.NPCs.Components;

namespace KArpRebornCORE.NPCs
{
    public class KArpNPC : GlobalNPC
    {

        public override void SetDefaults(NPC npc)
        {
            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            npc.lifeMax = (int)(npc.lifeMax * config.EnemyDifficultyMod);
            npc.damage = (int)(npc.damage * config.EnemyDifficultyMod);
        }

        public override bool PreAI(NPC npc) {
            NPCComponentSystem npccs = npc.GetGlobalNPC<NPCComponentSystem>();
            if (npccs == null) {
                KArpRebornCOREMain.Mod.Logger.Warn("NPC " + npc.FullName + " does not have an NPCComponentSystem, aborting!");
                return true;
            }

            NPCStatDef npcsd = npccs.TryGetComponent<NPCStatDef>(() => new NPCStatDef());

            npc.GivenName = $"Lv. {npcsd.GetLevel()} {npc.TypeName}";
            return true;
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            NPCComponentSystem npccs = npc.GetGlobalNPC<NPCComponentSystem>();
            if (npccs == null)
            {
                KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have an NPCComponentSystem attached!", npc.FullName);
                return;
            }

            KArpPlayer c = target.GetModPlayer<KArpPlayer>();

            if (c == null)
            {
                KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have a KArpPlayer attached!", target.name);
                return;
            }

            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            if (config.DoPlayerEvasion) {
                NPCStatDef npcsd = npccs.TryGetComponent<NPCStatDef>(() => new NPCStatDef());

                if (NPCHelper.CalcDodge(npcsd.GetAccuracy(), c.totalEvasion)) {
                    damage = 0;
                    crit = false;
                    CombatText.NewText(target.getRect(), Color.Green, "Evaded!");
                }
            }

            NPCCombatTracker npcct = npccs.TryGetComponent<NPCCombatTracker>(() => new NPCCombatTracker());
            npcct.AddPlayer(target);
            Networking.NPCCTSyncPacket.Write(npc.whoAmI, target.whoAmI);
        }

        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            if (!config.DoEnemyEvasion)
                return;

            NPCComponentSystem attackerNPCCS = npc.GetGlobalNPC<NPCComponentSystem>();
            if (attackerNPCCS == null)
            {
                KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have an NPCComponentSystem attached!", npc.FullName);
                return;
            }

            NPCComponentSystem defenderNPCCS = target.GetGlobalNPC<NPCComponentSystem>();
            if (defenderNPCCS == null)
            {
                KArpRebornCOREMain.Mod.Logger.ErrorFormat("{} does not have an NPCComponentSystem attached!", target.FullName);
                return;
            }

            NPCStatDef attackerNPCSD = attackerNPCCS.TryGetComponent<NPCStatDef>(() => new NPCStatDef());
            NPCStatDef defenderNPCSD = defenderNPCCS.TryGetComponent<NPCStatDef>(() => new NPCStatDef());

            if (NPCHelper.CalcDodge(attackerNPCSD.GetAccuracy(), defenderNPCSD.GetEvasion())) {
                damage = 0;
                crit = false;
                CombatText.NewText(target.getRect(), Color.Green, "Evaded!");
            }
        }

        public override void NPCLoot(NPC npc)
        {

            //Check that we are a valid enemy
            if (npc.lifeMax < 10) return;
            if (npc.friendly) return;
            if (npc.townNPC) return;

            NPCComponentSystem npccs = npc.GetGlobalNPC<NPCComponentSystem>();
            //Check if someone has put an NPCStatDef on this NPC
            NPCStatDef statDef = npccs?.GetComponent<NPCStatDef>();

            //If not, create a default one
            if (statDef == null)
            {
                //We don't bother adding the NPCStatDef since this NPC will get destroyed
                statDef = new NPCStatDef();
                statDef.npc = npc;
            }

            int level = statDef.GetLevel();
            int experience = statDef.GetExperience();

            NPCCombatTracker npcct = npccs?.GetComponent<NPCCombatTracker>();
            if (npcct == null) {
                KArpRebornCOREMain.Mod.Logger.Debug("No NPCCT available, aborting");
                return;
            }
            
            Player[] combatants = npcct.GetPlayers();
            if (Main.netMode == NetmodeID.SinglePlayer && combatants.Length > 0)
            {
                Main.LocalPlayer.GetModPlayer<KArpPlayer>().AddXp(statDef.GetExperience());
            } else if (Main.netMode == NetmodeID.Server) {
                foreach(Player player in combatants)
                {
                    Networking.AddXPPacket.Write(statDef.GetExperience(), player.whoAmI);
                }
            }
        }
    }
}
