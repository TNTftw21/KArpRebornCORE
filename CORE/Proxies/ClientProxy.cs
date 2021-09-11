using System;
using System.Collections.Generic;

using Terraria;
using Terraria.UI;

using Microsoft.Xna.Framework;

using KArpReborn.CORE.UIs;

namespace KArpReborn.CORE.Proxies
{
    // All client-side only code gets called from here. This keeps the main mod class much cleaner.
    public class ClientProxy : ServerProxy
    {
		internal UIs.StatUI.StatMenu statMenu;
		internal UserInterface _statMenu;
        internal UIs.LevelBarUI.LevelBarState levelBar;
        internal UserInterface _levelBar;

        public override void Load()
        {
            base.Load();
            GFX.LoadGfx();
            statMenu = new UIs.StatUI.StatMenu();
            _statMenu = new UserInterface();
            levelBar = new UIs.LevelBarUI.LevelBarState();
            _levelBar = new UserInterface();
            _levelBar.SetState(levelBar);
        }

        public override void Unload()
        {
            base.Unload();
            GFX.UnloadGfx();
        }

        public void UpdateUI(GameTime gameTime)
        {
            _statMenu?.Update(gameTime);
            _levelBar?.Update(gameTime);
        }

        public void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarsIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));

            if (resourceBarsIndex != -1)
            {
                layers.Insert(resourceBarsIndex, new LegacyGameInterfaceLayer(
                    "kArpReborn: Level Bar",
                    delegate {
                        _levelBar.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI
                ));
            }

            int invIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                
            if (Main.playerInventory && invIndex != -1)
            {
                if (_statMenu.CurrentState == null)
                {
                    _statMenu.SetState(statMenu);
                }
                layers.Insert(invIndex, new LegacyGameInterfaceLayer(
                    "kArpReborn: Stat Menu",
                    delegate {
                        _statMenu.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI
                ));
            } else
            {
                _statMenu.SetState(null);
            }
        }
    }
}
