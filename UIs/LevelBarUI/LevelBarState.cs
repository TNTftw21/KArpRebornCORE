using System;

using Terraria.UI;

namespace KArpRebornCORE.UIs.LevelBarUI
{
    public class LevelBarState : UIState
    {

        private LevelBar levelBar;

        public override void OnInitialize()
        {
            this.levelBar = new LevelBar();
            this.levelBar.HAlign = 0;
            this.levelBar.VAlign = 1;
            this.levelBar.MarginBottom = this.levelBar.Width.Pixels;

            Append(this.levelBar);
        }
    }
}
