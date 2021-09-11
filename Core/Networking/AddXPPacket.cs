using System;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria.ModLoader;

namespace KArpReborn.Core.Networking
{
    public class AddXPPacket
    {

        public static void Read(BinaryReader reader)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Players.KArpPlayer c = Main.LocalPlayer.GetModPlayer<Players.KArpPlayer>();
                c.AddXp((int)reader.ReadInt32());
            }
        }

        public static bool Write(int scaled, int target)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = KArpRebornMain.Mod.GetPacket();
                packet.Write((byte)Message.AddXp);
                packet.Write(scaled);
                packet.Send(target);
                return true;
            }
            return false;
        }
    }
}
