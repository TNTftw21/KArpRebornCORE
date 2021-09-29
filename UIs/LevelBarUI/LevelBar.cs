using System;

using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using KArpRebornCORE.Players;

namespace KArpRebornCORE.UIs.LevelBarUI
{
    public class LevelBar : UIElement
    {

        private UIText levelText;
        private int level = 1;

        public override void OnInitialize()
        {
            this.Width.Set((int)(GFX.LevelBar.Width * 1.5), 0);
            // Half of this texture is the outline, the other half is the bar filling.
            this.Height.Set((int)(GFX.LevelBar.Height * 0.75), 0);
            levelText = new UIText("1", 1.5f, false);
            levelText.MarginLeft = ((this.Width.Pixels * 60 / 210) - levelText.MinWidth.Pixels) / 2;
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
            base.Update(gameTime);
            KArpPlayer player = Main.LocalPlayer.GetModPlayer<KArpPlayer>();
            if (this.level != player.level) {
                levelText.SetText(player.level.ToString());
                float width = this.Width.Pixels;
                float textWidth = levelText.MinWidth.Pixels;
                levelText.MarginLeft = ((width * 60 / 210) - textWidth) / 2;
                this.level = player.level;
            }
        }

    }
}
