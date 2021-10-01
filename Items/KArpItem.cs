using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

using KArpRebornCORE.Prefixes;
using KArpRebornCORE.Players;

namespace KArpRebornCORE.Items
{
    public class KArpItem : GlobalItem
    {

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Mod mod = KArpRebornCOREMain.Mod;
            ModPrefix prefix = ModPrefix.GetPrefix(item.prefix);
            if (item.vanity || prefix != null) {
                TooltipLine line;
                if (prefix.GetType() == typeof(EvasivePrefix))
                    line = new TooltipLine(mod, "EvasivePrefix", $"+{(prefix as EvasivePrefix)._power} evasion");
                else if (prefix.GetType() == typeof(AccuratePrefix))
                    line = new TooltipLine(mod, "AccuratePrefix", $"+{(prefix as AccuratePrefix)._power} accuracy");
                else
                    return;
                line.isModifier = true;
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            KArpPlayer c = player.GetModPlayer<KArpPlayer>();
            ModPrefix prefix = ModPrefix.GetPrefix(item.prefix);
            if (item.vanity || prefix == null)
                return;
            
            if (prefix.GetType() == typeof(EvasivePrefix))
                c.baseEvasion += (prefix as EvasivePrefix)._power;
            if (prefix.GetType() == typeof(AccuratePrefix))
                c.baseAccuracy += (prefix as AccuratePrefix)._power;
        }

    }
}