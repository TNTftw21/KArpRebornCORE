using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KArpRebornCORE.NPCs.Components
{
    public class NPCStatDef : Component
    {

        public virtual int GetLevel()
        {
            return Math.Min((npc.damage + npc.defense * 4) / 3, npc.boss ? 120 : 110);
        }

        public virtual int GetExperience()
        {
            int life = npc.type == NPCID.SolarCrawltipedeBody || npc.type == NPCID.SolarCrawltipedeHead || npc.type == NPCID.SolarCrawltipedeTail
                ? npc.lifeMax / 8
                : npc.lifeMax;
        
            int defFactor = npc.defense < 0 ? 1 : npc.defense * life / (10);
            int baseExp = Main.rand.Next((life + defFactor / 5)) + (life + defFactor) / 6;
            //Since we scale XP off of enemy strength, we have to take into account expert mode scaling.
            //Otherwise, we end up gaining more XP because we're in expert and completely negate the extra challenge
            return (int)(baseExp * (1 / KArpRebornCOREMain.Mod.DifficultyFactor));
        }

        public virtual int GetAccuracy()
        {
            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            return config.EnemyAccuracyBase + (config.EnemyAccuracyGrowth * GetLevel());
        }

        public virtual int GetEvasion()
        {
            KArpConfigServer config = ModContent.GetInstance<KArpConfigServer>();
            return config.EnemyEvasionBase + (config.EnemyEvasionGrowth * GetLevel());
        }
    }
}
