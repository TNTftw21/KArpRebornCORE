using System;
using System.Collections.Generic;

using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

using KArpRebornCORE.Players;
using KArpRebornCORE.NPCs;
using KArpRebornCORE.NPCs.Components;

namespace KArpRebornCORE.UIs.CharacterScreenUI {
    public class CharacterScreenState : UIState
    {

        private static float newLineDist = 7;
    
        private UIPanel panel;
        private string text;
        
        public override void OnInitialize()
        {
            panel = new UIPanel();
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);
        }
    
        public override void OnActivate()
        {
            base.OnActivate();
            panel.RemoveAllChildren();
            Main.playerInventory = false;
            KArpPlayer player = Main.LocalPlayer.GetModPlayer<KArpPlayer>();
            FakeStatDef fakeDef = new FakeStatDef(player.level);
            text = 
                $"Life: {player.player.statLifeMax2}\n" +
                $"Mana: {player.player.statManaMax2}\n" +
                $"Defense: {player.player.statDefense}\n" +
                $"Evasion: {player.totalEvasion}\n" +
                $"Estimated chance to evade: {(NPCHelper.CalcDodgeChance(fakeDef.GetAccuracy(), player.totalEvasion) * 100f)}%\n" +
                $"Accuracy: {player.totalAccuracy}\n" +
                $"Estimated chance to hit: {(1 - NPCHelper.CalcDodgeChance(player.totalAccuracy, fakeDef.GetEvasion()))*100f}%\n" +
                $"Movement speed: {player.player.moveSpeed}\n" +
                $"Melee speed: {1 / player.player.meleeSpeed}\n" +
                $"Melee damage increase: {(player.player.meleeDamage - 1 + player.player.allDamage - 1) * 100}%\n" +
                $"Ranged damage increase: {(player.player.rangedDamage - 1 + player.player.allDamage - 1) * 100}%\n" +
                $"Magic damage increase: {(player.player.magicDamage - 1 + player.player.allDamage - 1) * 100}%\n" +
                $"Throwing damage increase: {(player.player.thrownDamage - 1 + player.player.allDamage - 1) * 100}%\n";
            float minWidth = 0;
            float minHeight = 0;
            string temp = "";
            foreach (char c in text) {
                if (c == '\n') {
                    UIText _ = new UIText(temp);
                    _.Top.Set(minHeight, 0);
                    minWidth = Math.Max(minWidth, _.MinWidth.Pixels);
                    minHeight += _.MinHeight.Pixels + newLineDist;
                    temp = "";
                    panel.Append(_);
                } else
                    temp += c;
            }
            minWidth += panel.PaddingLeft + panel.PaddingRight;
            minHeight += panel.PaddingTop * 2;
            panel.MinWidth.Set(minWidth, 0);
            panel.MinHeight.Set(minHeight, 0);
            panel.Recalculate();
        }
    
        private class FakeStatDef : NPCStatDef {
        
            private int level = 1;
    
            public FakeStatDef(int level) : base()
            {
                this.level = level;
            }
    
            public override int GetLevel()
            {
                return level;
            }
        }
    }
}