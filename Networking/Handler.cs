using System;
using System.IO;

namespace KArpRebornCORE.Networking
{
    public class Handler
    {
        public static void HandlePacket(BinaryReader reader, int whoAmI)
        {
            Message msg = (Message)reader.ReadByte();
            switch (msg)
            {
                case Message.AddXp:
                    AddXPPacket.Read(reader);
                    break;
                case Message.SyncPlayer:
                    SyncPlayerPacket.Read(reader);
                    break;
                case Message.NPCCTSync:
                    NPCCTSyncPacket.Read(reader);
                    break;
            }
        }
    }
}
