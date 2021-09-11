using System;

using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

using Terraria.ModLoader.UI;

using Microsoft.Xna.Framework;

namespace KArpReborn.Core.UIs
{
    public class UITextButton : UIText
    {

        private readonly float duration = 0.15f;
        private float timer = 0.0f;
        private bool grow = false;

        public UITextButton(string text) : base(text)
        {
            this.OnMouseOver += MouseEnter;
            this.OnMouseOut += MouseExit;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (timer > 0)
            {
                timer -= (float)gameTime.ElapsedGameTime.Milliseconds/1000f;
                float percentAnimation;
                if (grow)
                {
                    percentAnimation = 1 - (timer / duration);
                }
                else
                {
                    percentAnimation = timer / duration;
                }
                this.SetText(this.Text, 1 + (.2f * percentAnimation), false);
            }
        }

        public void MouseEnter(UIMouseEvent mouseEvent, UIElement element)
        {
            timer = duration;
            grow = true;
        }

        public void MouseExit(UIMouseEvent mouseEvent, UIElement element)
        {
            timer = duration;
            grow = false;
        }

    }
}
