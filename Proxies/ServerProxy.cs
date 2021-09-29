using System;

namespace KArpRebornCORE.Proxies
{
    public class ServerProxy
    {

        public virtual void Load()
        {

        }

        public virtual void Unload()
        {
            
        }

        public virtual void PostSetupContent()
        {
            KArpConfigServer config = Terraria.ModLoader.ModContent.GetInstance<KArpConfigServer>();
            KArpRebornCOREMain.Mod.difficultyFactorTracker.Add("CORE Config", config.EnemyDifficultyMod);
        }
    }
}
