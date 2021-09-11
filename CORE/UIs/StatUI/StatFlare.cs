using System;

using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KArpReborn.CORE.UIs.StatUI
{
    public class StatFlare : UIElement
    {
        internal Players.PlayerStats stat;
        internal Players.KArpPlayer krpgPlayer;
        internal int[] tempPoints;
        private UITextButton uiText;
        private Color allocatedColor = new Color(255, 187, 0);

        //Number of frames to show each animation frame
        private int animationTime = 5;
        private int numFrames = 0;

        public StatFlare(Players.PlayerStats stat) : base()
        {
            this.stat = stat;
        }

        public override void OnInitialize()
        {
            uiText = new UITextButton("0");
            uiText.HAlign = 0.5f;
            uiText.VAlign = 0.5f;
            this.Width = new StyleDimension(GFX.Flames[(int)stat].Width/2, 0f);
            //Image consists of 8 frames oriented vertically, so we use 1/8 the height, then divide by 2 gives us 1/16
            this.Height = new StyleDimension(GFX.Flames[(int)stat].Height/16, 0f);
            Append(uiText);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (numFrames > 8 * animationTime - 1) numFrames = 0;
            int frameNumber = (int) Math.Floor(numFrames / (double)animationTime);
            CalculatedStyle calcStyle = GetDimensions();
            Point point1 = new Point((int)calcStyle.X, (int)calcStyle.Y);
            int width = (int)Math.Ceiling(calcStyle.Width);
            int height = (int)Math.Ceiling(calcStyle.Height);
            spriteBatch.Draw(UIs.GFX.Flames[(int)this.stat], new Rectangle(point1.X, point1.Y, width, height), new Rectangle(0, frameNumber * 68, 56, 68), Color.White);
            numFrames++;
            if (IsMouseHovering || uiText.IsMouseHovering) {
                switch (this.stat) {
                    case Players.PlayerStats.Resilience:
                        Main.hoverItemName = "Resilience\n+10 Life, +1 Defense";
                        break;
                    case Players.PlayerStats.Quickness:
                        Main.hoverItemName = "Quickness\n5% increased Accuracy, Evasion, and Movement and Melee Speed";
                        break;
                    case Players.PlayerStats.Potency:
                        Main.hoverItemName = "Potency\n5% increased Damage, 0.2% of Damage Leeched as Life";
                        break;
                }
            }
        }

        public override void Click(UIMouseEvent evt)
        {
            if (krpgPlayer.totalPoints - (krpgPlayer.spentPoints + tempPoints[0] + tempPoints[1] + tempPoints[2]) > 0) {
                tempPoints[(int)stat]++;
                Main.PlaySound(Terraria.ID.SoundID.MenuTick);
            }
        }

        public override void RightClick(UIMouseEvent evt)
        {
            if (tempPoints[(int)stat] > 0)
                tempPoints[(int)stat]--;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            int statAlloc = krpgPlayer.Stats[(int) stat] + tempPoints[(int)stat];
            uiText.SetText(statAlloc.ToString());
            if (tempPoints[(int)stat] > 0)
                uiText.TextColor = this.allocatedColor;
            else
                uiText.TextColor = Color.White;
        }

        public override void OnActivate()
        {
            base.OnActivate();
            this.krpgPlayer = Main.LocalPlayer.GetModPlayer<Players.KArpPlayer>();
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            this.uiText.SetText("0");
        }
    }
}
