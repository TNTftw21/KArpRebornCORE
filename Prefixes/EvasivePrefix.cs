using System;

using Terraria;
using Terraria.ModLoader;

namespace KArpRebornCORE.Prefixes
{
    public class EvasivePrefix : ModPrefix
    {
        internal readonly byte _power;

        public override float RollChance(Item item) => 5f;

        public override bool CanRoll(Item item) => true;

        public override PrefixCategory Category => PrefixCategory.Accessory;

        public EvasivePrefix() {

        }

        public EvasivePrefix(byte _power) {
            this._power = _power;
        }

        public override bool Autoload(ref string name)
        {
            if (!base.Autoload(ref name))
                return false;
            
            KArpRebornCOREMain.Mod.AddPrefix("Evasive", new EvasivePrefix(1));
            KArpRebornCOREMain.Mod.AddPrefix("Elusive", new EvasivePrefix(2));
            KArpRebornCOREMain.Mod.AddPrefix("Agile", new EvasivePrefix(3));
            KArpRebornCOREMain.Mod.AddPrefix("Deft", new EvasivePrefix(4));
            return false;
        }

        public override void Apply(Item item)
        {
            Main.player[item.owner].GetModPlayer<Players.KArpPlayer>().baseEvasion += this._power;
        }

        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = 1f + 0.05f * _power;
            valueMult *= multiplier;
        }
    }
}
