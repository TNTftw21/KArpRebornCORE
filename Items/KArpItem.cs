using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace KArpRebornCORE.Items
{
    public class KArpItem : GlobalItem
    {

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Mod mod = KArpRebornCOREMain.Mod;
            ModPrefix prefix = ModPrefix.GetPrefix(item.prefix);
            if (prefix != null) {
                if (prefix.GetType() == typeof(Prefixes.EvasivePrefix))
                    tooltips.Add(new TooltipLine(mod, "EvasivePrefix", $"+{(prefix as Prefixes.EvasivePrefix)._power} evasion"));
                else if (prefix.GetType() == typeof(Prefixes.AccuratePrefix))
                    tooltips.Add(new TooltipLine(mod, "AccuratePrefix", $"+{(prefix as Prefixes.AccuratePrefix)._power} accuracy"));
            }
        }

    }
}