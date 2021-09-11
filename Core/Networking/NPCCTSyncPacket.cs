using System;
using System.IO;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using KArpReborn.Core.NPCs;

namespace KArpReborn.Core.Networking
{
    public class NPCCTSyncPacket
    {
        public static void Read(BinaryReader reader)
        {
            NPC npc = Main.npc[reader.ReadInt32()];
            NPCComponentSystem npccs = npc.GetGlobalNPC<NPCComponentSystem>();
            npccs.TryGetComponent<NPCs.Components.NPCCombatTracker>(() => new NPCs.Components.NPCCombatTracker()).AddPlayer(Main.player[reader.ReadInt32()]);
        }

        public static void Write(int npcTarget, int player)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            
            ModPacket packet = KArpRebornMain.Mod.GetPacket();
            packet.Write((byte)Message.NPCCTSync);
            packet.Write(npcTarget);
            packet.Write(player);
            packet.Send();
        }
    }
}
