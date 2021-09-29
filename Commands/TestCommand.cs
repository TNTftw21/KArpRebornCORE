using System;

using Terraria;
using Terraria.ModLoader;

namespace KArpRebornCORE.Commands
{
    public class TestCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;

        public override string Command => "test";

        public override string Usage => "/test";

        public override string Description => "KArpReborn test command";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (Main.netMode == Terraria.ID.NetmodeID.Server || caller.Player != Main.LocalPlayer)
                return;
            Main.NewText("Current DifficultyFactor: " + KArpRebornCOREMain.Mod.DifficultyFactor);
        }
    }
}
