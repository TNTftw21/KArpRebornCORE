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

        [Header("Evasion")]
        [DefaultValue(true)]
        public bool DoEnemyEvasion;
        [DefaultValue(true)]
        public bool DoPlayerEvasion;

        [Header("Leveling")]
        [Tooltip("How much health a player gains per level.")]
        [DefaultValue(5)]
        public int PlayerHealthGrowth;
        [Tooltip("How much mana a player gains per level.")]
        [DefaultValue(0)]
        public int PlayerManaGrowth;
        [Tooltip("How much base evasion a player should have.\nA level 1 player will have this plus PlayerEvasionGrowth.")]
        [DefaultValue(3)]
        public int PlayerEvasionBase;
        [Tooltip("How much evasion a player gains passively per level.")]
        [DefaultValue(2)]
        public int PlayerEvasionGrowth;
        [Tooltip("How much base accuracy a player should have.\nA level 1 player will have this plus PlayerAccuracyGrowth")]
        [DefaultValue(3)]
        public int PlayerAccuracyBase;
        [Tooltip("How much accuracy a player gains passively per level.")]
        [DefaultValue(2)]
        public int PlayerAccuracyGrowth;

        [Tooltip("How much base evasion an enemy should have.\nA level 1 enemy will have this plus EnemyEvasionGrowth")]
        [DefaultValue(3)]
        public int EnemyEvasionBase;
        [Tooltip("How much additional evasion an enemy gains per level")]
        [DefaultValue(5)]
        public int EnemyEvasionGrowth;
        [Tooltip("How much base accuracy an enemy should have.\nA level 1 enemy will have this plus EnemyEvasionGrowth")]
        [DefaultValue(3)]
        public int EnemyAccuracyBase;
        [Tooltip("How much additional accuracy an enemy gains per level")]
        [DefaultValue(5)]
        public int EnemyAccuracyGrowth;

        [Header("Stat Effects")]
        [Tooltip("Determines the health each point of Resilience gives.")]
        [DefaultValue(10)]
        public int ResilienceHealthPerPoint;
        [Tooltip("Determines the defense each point of Resilience gives.")]
        [DefaultValue(1)]
        public int ResilienceDefensePerPoint;
        [Tooltip("Determines the percent increase in accuracy each point of Quickness gives.")]
        [DefaultValue(0.05f)]
        public float QuicknessAccuracyPerPoint;
        [Tooltip("Determines the percent increase in evasion each point of Quickness gives.")]
        [DefaultValue(0.05f)]
        public float QuicknessEvasionPerPoint;
        [Tooltip("Determines the percent increase in movement speed each point of Quickness gives.")]
        [DefaultValue(0.05f)]
        public float QuicknessMovementPerPoint;
        [Tooltip("Determines the percent increase in attack speed each point of Quickness gives.")]
        [DefaultValue(0.05f)]
        public float QuicknessAttackSpeedPerPoint;
        [Tooltip("Determines the mana each point of Wits gives")]
        [DefaultValue(3)]
        public int WitsManaPerPoint;
        [Tooltip("Determines the percent increase in damage each point of Wits gives")]
        [DefaultValue(0.05f)]
        public float WitsDamagePerPoint;
        [Tooltip("Determines the percent of Life Leech each point of Wits gives")]
        [DefaultValue(0.002)]
        public float WitsLeechPerPoint;
    }
}
