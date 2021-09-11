using System;

using Terraria;
using Terraria.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KArpReborn.Core.UIs.StatUI
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
