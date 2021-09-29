using System;

namespace KArpRebornCORE
{
    public class NPCHelper
    {
        public static bool CalcDodge(float accuracy, float evasion) {
            float chanceToEvade = CalcDodgeChance(accuracy, evasion);
            return new Random().NextDouble() <= chanceToEvade;
        }

        public static float CalcDodgeChance(float accuracy, float evasion) {
            return evasion / (evasion + (3 * accuracy));
        }
    }
}
