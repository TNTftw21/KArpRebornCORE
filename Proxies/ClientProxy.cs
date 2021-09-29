using System;
using System.Collections.Generic;

using Terraria;
using Terraria.UI;

using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using KArpRebornCORE.UIs;

namespace KArpRebornCORE.Proxies
{
    // All client-side only code gets called from here. This keeps the main mod class much cleaner.
    public class ClientProxy : ServerProxy
    {
		internal UIs.StatUI.StatMenu statMenu;
		internal UserInterface _statMenu;
        internal UIs.LevelBarUI.LevelBarState levelBar;
        internal UserInterface _levelBar;
        internal UIs.CharacterScreenUI.CharacterScreenState characterScreen;
        internal UserInterface _characterScreen;
        public ModHotKey characterScreenHotkey;

        public override void Load()
        {
            base.Load();
			characterScreenHotkey = KArpRebornCOREMain.Mod.RegisterHotKey("Character Screen", "C");
            GFX.LoadGfx();
            statMenu = new UIs.StatUI.StatMenu();
            _statMenu = new UserInterface();
            levelBar = new UIs.LevelBarUI.LevelBarState();
            _levelBar = new UserInterface();
            _levelBar.SetState(levelBar);
            characterScreen = new UIs.CharacterScreenUI.CharacterScreenState();
            _characterScreen = new UserInterface();
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
            if (Main.playerInventory)
                _characterScreen.SetState(null);
            _characterScreen?.Update(gameTime);
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
                
            if (invIndex != -1)
            {
                layers.Insert(invIndex, new LegacyGameInterfaceLayer(
                    "kArpReborn: Character Screen",
                    delegate {
                        _characterScreen.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI
                ));
                if (Main.playerInventory) {
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
}
