using System;

using Terraria.UI;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KArpReborn.CORE.UIs.StatUI
{
    public class StatDeerSkull : UIElement
    {

        internal int currentStat = -1;

        public StatDeerSkull() : base()
        {
            this.Width = new StyleDimension(GFX.DeerSkull.Width, 0f);
            this.Height = new StyleDimension(GFX.DeerSkull.Height, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle calcStyle = GetDimensions();
            Point point1 = new Point((int)calcStyle.X, (int)calcStyle.Y);
            int width = (int) Math.Ceiling(calcStyle.Width);
            int height = (int) Math.Ceiling(calcStyle.Height);
            spriteBatch.Draw(GFX.DeerSkull, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            if (currentStat != -1)
                spriteBatch.Draw(GFX.DeerSkullEyes[currentStat], new Rectangle(point1.X, point1.Y, width, height), Color.White);
        }
    }
}
