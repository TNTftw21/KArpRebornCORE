using System;

using Terraria;
using Terraria.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KArpRebornCORE.UIs.StatUI
{
    public class StatMenu : UIState
    {
        private StatMenuContainer container;

        public override void OnInitialize()
        {
            container = new StatMenuContainer();
            Append(container);
        }
    }
}
