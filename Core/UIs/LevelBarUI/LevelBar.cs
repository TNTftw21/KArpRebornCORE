using System;

using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using KArpReborn.Core.Players;

namespace KArpReborn.Core.UIs.LevelBarUI
{
    public class LevelBar : UIElement
    {

        private UIText levelText;

        public override void OnInitialize()
        {
            this.Width.Set((int)(GFX.LevelBar.Width * 1.5), 0);
            // Half of this texture is the outline, the other half is the bar filling.
            this.Height.Set((int)(GFX.LevelBar.Height * 0.75), 0);
            levelText = new UIText("1");
            levelText.SetText(levelText.Text, 1.5f, false);
            levelText.HAlign = 15 / 140f; // We want it centered on (20, 20), and our image is 140 across. 20/140 = 1/7
            levelText.VAlign = 0.5f;
            Append(levelText);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            KArpPlayer player = Main.LocalPlayer.GetModPlayer<KArpPlayer>();
            CalculatedStyle dims = this.GetDimensions();
            spriteBatch.Draw(GFX.LevelBar, new Rectangle((int)dims.X, (int)dims.Y, (int)dims.Width, (int)dims.Height), new Rectangle(0, 0, 140, 40), Color.White);
            float percent = player.experience / (float)player.ExperienceToLevel();
            spriteBatch.Draw(GFX.LevelBar, new Rectangle((int)dims.X, (int)dims.Y, (int)(dims.Width * percent), (int)dims.Height), new Rectangle(0, 41, (int)(140 * percent), 39), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            KArpPlayer player = Main.LocalPlayer.GetModPlayer<KArpPlayer>();
            levelText.SetText(player.level.ToString());
            base.Update(gameTime);
        }

    }
}
