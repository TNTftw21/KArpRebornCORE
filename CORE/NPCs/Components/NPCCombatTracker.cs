using System.Collections.Generic;

using Terraria;

namespace KArpReborn.CORE.NPCs.Components
{
    public class NPCCombatTracker : Component
    {
        private List<Player> combatants = new List<Player>();

        public void AddPlayer(Player player) {
            if (!combatants.Contains(player)) {
                this.combatants.Add(player);
            }
        }

        public void RemovePlayer(Player player) {
            this.combatants.Remove(player);
        }

        public Player[] GetPlayers() {
            return combatants.ToArray();
        }
    }
}
