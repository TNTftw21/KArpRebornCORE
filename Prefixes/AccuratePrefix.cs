using System;

using Terraria;
using Terraria.ModLoader;

namespace KArpRebornCORE.Prefixes
{
    public class AccuratePrefix : ModPrefix
    {
        internal readonly byte _power;

        public override float RollChance(Item item) => 1f;

        public override bool CanRoll(Item item) => true;

        public override PrefixCategory Category => PrefixCategory.Accessory;

        public AccuratePrefix() {

        }

        public AccuratePrefix(byte _power) {
            this._power = _power;
        }

        public override bool Autoload(ref string name)
        {
            if (!base.Autoload(ref name))
                return false;
            
            KArpRebornCOREMain.Mod.AddPrefix("Accurate", new AccuratePrefix(1));
            KArpRebornCOREMain.Mod.AddPrefix("Targeted", new AccuratePrefix(2));
            KArpRebornCOREMain.Mod.AddPrefix("Uncanny", new AccuratePrefix(3));
            KArpRebornCOREMain.Mod.AddPrefix("Pinpoint", new AccuratePrefix(4));
            return false;
        }

        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = 1f + 0.05f * _power;
            valueMult *= multiplier;
        }
    }
}
