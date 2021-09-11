using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;


namespace KArpReborn.CORE
{
    public class KArpConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        public bool DoEvasion;

        [Header("Stat Effects")]
        [Tooltip("Determines the percent increase in accuracy each point of Quickness gives.")]
        [DefaultValue(0.05f)]
        public float QuicknessAccuracyPerPoint;

        //We don't *necessarily* need to reload after every change, but this ensures consistency.
        public override bool NeedsReload(ModConfig pendingConfig)
        {
            return true;
        }
    }
}
