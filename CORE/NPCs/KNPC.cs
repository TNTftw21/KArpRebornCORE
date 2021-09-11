using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using KArpReborn.CORE.Players;
using KArpReborn.CORE.NPCs.Components;

namespace KArpReborn.CORE.NPCs
{
    public class KNPC : GlobalNPC
    {

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

            NPCStatDef npcsd = npccs.TryGetComponent<NPCStatDef>(() => new NPCStatDef());

            float accuracy = npcsd.GetAccuracy();
            float evasion = c.GetEvasion();

            //Asymptotic curve, where if (evasion == accuracy) then chanceToEvade = 0.25.
            //The higher your evasion/accuracy, the less each point actually does.
            float chanceToEvade = evasion / (evasion + (3 * accuracy));
            Random rand = new Random();
            double d = rand.NextDouble();
            if (d <= chanceToEvade) {
                damage = 0;
                crit = false;
                CombatText.NewText(target.getRect(), Color.Green, "Evaded!");
            }

            NPCCombatTracker npcct = npccs.TryGetComponent<NPCCombatTracker>(() => new NPCCombatTracker());
            npcct.AddPlayer(target);
            Networking.NPCCTSyncPacket.Write(target.whoAmI, target.whoAmI);
        }

        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
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

            float accuracy = attackerNPCSD.GetAccuracy();
            float evasion = defenderNPCSD.GetEvasion();

            float chanceToEvade = evasion / (evasion + (3 * accuracy));
            Random rand = new Random();
            double d = rand.NextDouble();
            if (d <= chanceToEvade) {
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
                    KArpRebornCOREMain.Mod.Logger.Debug("Awarding experience to " + player.name);
                    Networking.AddXPPacket.Write(statDef.GetExperience(), player.whoAmI);
                }
            }
        }
    }
}
