using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace KArpRebornCORE.UIs
{
    public class GFX
    {
        private const string GUI_DIRECTORY = "Assets/GUI/";
        public static Texture2D DeerSkull { get; private set; }
        public static Texture2D[] Flames;
        public static Texture2D[] DeerSkullEyes;
        public static Texture2D LevelBar { get; private set; }

        public static void LoadGfx()
        {
            Flames = new Texture2D[(int)Players.PlayerStats._END];
            DeerSkullEyes = new Texture2D[Flames.Length];
            Mod loader = KArpRebornCOREMain.Mod;

            DeerSkull = loader.GetTexture(GUI_DIRECTORY + "DeerSkull");
            Players.PlayerStats stat = Players.PlayerStats.Resilience;
            Flames[(int)stat] = loader.GetTexture(GUI_DIRECTORY + "Flames_"+stat.ToString());
            DeerSkullEyes[(int)stat] = loader.GetTexture(GUI_DIRECTORY + "DeerSkull_Eyes_" + stat.ToString());
            stat = Players.PlayerStats.Quickness;
            Flames[(int)stat] = loader.GetTexture(GUI_DIRECTORY + "Flames_"+stat.ToString());
            DeerSkullEyes[(int)stat] = loader.GetTexture(GUI_DIRECTORY + "DeerSkull_Eyes_" + stat.ToString());
            stat = Players.PlayerStats.Potency;
            Flames[(int)stat] = loader.GetTexture(GUI_DIRECTORY + "Flames_"+stat.ToString());
            DeerSkullEyes[(int)stat] = loader.GetTexture(GUI_DIRECTORY + "DeerSkull_Eyes_" + stat.ToString());
            LevelBar = loader.GetTexture(GUI_DIRECTORY + "LevelBar");
            }

        public static void UnloadGfx()
        {
            DeerSkull = null;
            for (int i = 0; i < Flames.Length; i++)
                Flames[i] = null;
            Flames = null;
            for (int i = 0; i < DeerSkullEyes.Length; i++)
                DeerSkullEyes[i] = null;
            DeerSkullEyes = null;
            LevelBar = null;
        }
    }
}
