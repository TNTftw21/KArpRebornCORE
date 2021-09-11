using System;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria.ModLoader;

using KArpReborn.Core.Players;

namespace KArpReborn.Core.Networking
{
    public class SyncPlayerPacket
    {

        public static void Read(BinaryReader reader)
        {
            if (Main.netMode == NetmodeID.Server) {
                KArpPlayer player = Main.player[reader.ReadInt32()].GetModPlayer<KArpPlayer>();
                player.level = reader.ReadInt32();
                int statsLength = reader.ReadInt32();
                statsLength = Math.Min(statsLength, (int)PlayerStats.Potency);
                for (int i = 0; i < statsLength; i++)
                    player.Stats[i] = reader.ReadInt32();
            }
        }

        public static void Write(int whoAmI, int level, int[] Stats)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = KArpRebornMain.Mod.GetPacket();
                packet.Write((byte) Message.SyncPlayer);
                packet.Write(whoAmI);
                packet.Write(Stats.Length);
                for (int i = 0; i < Stats.Length; i++)
                    packet.Write(Stats[i]);
                packet.Send();
            }
        }
    }
}
