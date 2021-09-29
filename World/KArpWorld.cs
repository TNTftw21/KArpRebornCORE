using System;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace KArpRebornCORE.World
{
    public class KArpWorld : ModWorld
    {

        public override void Initialize()
        {
            KArpRebornCOREMain.Mod.difficultyFactorTracker["World Difficulty"] = (Main.expertMode ? 2 : 1);
        }

    }
}
