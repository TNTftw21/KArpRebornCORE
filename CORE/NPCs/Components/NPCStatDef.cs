using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KArpReborn.CORE.NPCs.Components
{
    public class NPCStatDef : Component
    {

        public virtual int GetLevel()
        {
            return Math.Min(Main.expertMode ? (npc.damage + npc.defense * 4) / 3 : (npc.damage * 2 + npc.defense * 4) / 3, npc.boss ? 120 : 110);
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
            //TODO: I belive 1.4 makes world difficulty into an object that contains modifiers, so we can reference that instead
            return Main.expertMode ? (int)(baseExp * 0.5f) : baseExp;
        }

        public virtual int GetAccuracy()
        {
            return 3 + (5 * GetLevel());
        }

        public virtual int GetEvasion()
        {
            return 3 + (5 * GetLevel());
        }

        public virtual int GetResist(Element element)
        {
            return 0;
        }
    }
}
