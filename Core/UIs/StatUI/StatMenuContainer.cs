using System;

using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using KArpReborn.Core.Players;

namespace KArpReborn.Core.UIs.StatUI
{
    public class StatMenuContainer : UIElement
    {
        private StatDeerSkull skull;
        private StatFlare[] flares = new StatFlare[(int)Players.PlayerStats.Potency + 1];
        private int[] tempPoints = new int[(int)Players.PlayerStats.Potency + 1];
        private UIText pointsText;
        private UITextButton applyText;
        private KArpPlayer player;

        public override void OnInitialize()
        {
            this.HAlign = 0.5f;
            this.VAlign = 0.15f;

            skull = new StatDeerSkull();
            skull.HAlign = 0.5f;
            skull.VAlign = 0.5f;
            this.Width.Set(skull.Width.Pixels * 1.5f, 0f);
            this.Height.Set(skull.Height.Pixels * 1.5f, 0f);

            flares = new StatFlare[(int)Players.PlayerStats.Potency + 1];
            Players.PlayerStats stat = Players.PlayerStats.Resilience;
            flares[(int)stat] = new StatFlare(stat);
            flares[(int)stat].tempPoints = this.tempPoints;
            flares[(int)stat].HAlign = 0.23f;
            flares[(int)stat].VAlign = 0.09f;
            stat = Players.PlayerStats.Quickness;
            flares[(int)stat] = new StatFlare(stat);
            flares[(int)stat].tempPoints = this.tempPoints;
            flares[(int)stat].HAlign = 0.5f;
            flares[(int)stat].VAlign = 0.2f;
            stat = Players.PlayerStats.Potency;
            flares[(int)stat] = new StatFlare(stat);
            flares[(int)stat].tempPoints = this.tempPoints;
            flares[(int)stat].HAlign = 0.77f;
            flares[(int)stat].VAlign = 0.09f;

            pointsText = new UIText("Unspent points: 0");
            pointsText.HAlign = 0.15f;
            pointsText.VAlign = 0.75f;

            applyText = new UITextButton("Apply");
            applyText.HAlign = 0.5f;
            applyText.VAlign = 0.9f;
            applyText.OnClick += ApplyPoints;

            foreach (StatFlare flare in flares)
                Append(flare);

            

            Append(skull);
            Append(pointsText);
            Append(applyText);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            int stat = -1;
            for (int i = 0; i < flares.Length; i++)
            {
                if (flares[i].IsMouseHovering)
                {
                    stat = i;
                    break;
                }
            }
            skull.currentStat = stat;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch batch)
        {
            pointsText.SetText($"Unspent Points: {player.totalPoints - (player.spentPoints + tempPoints[0] + tempPoints[1] + tempPoints[2])}");
        }

        public override void OnActivate()
        {
            base.OnActivate();
            this.player = Main.LocalPlayer.GetModPlayer<KArpPlayer>();
        }

        public override void OnDeactivate()
        {
            for (int i = 0; i < tempPoints.Length; i++)
                tempPoints[i] = 0;
            base.OnDeactivate();
        }

        protected void ApplyPoints(UIMouseEvent mouseEvent, UIElement listener)
        {
            for (int i = 0; i < tempPoints.Length; i++)
            {
                this.player.Stats[i] += tempPoints[i];
                tempPoints[i] = 0;
            }
            Main.PlaySound(Terraria.ID.SoundID.MenuTick);
        }

    }
} 