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


namespace KArpRebornCORE
{
    public class KArpConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        //I wish this didn't require a reload, but it's the only way to ensure that KArpNPC's DifficultyFactor gets updated
        [ReloadRequired]
        [DefaultValue(1.0f)]
        [Range(0.5f, 2f)]
        [Tooltip("How much to multiply enemy health and damage by, for balancing purposes.")]
        public float EnemyDifficultyMod;

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
        [Tooltip("Determines the percent increase in damage each point of Potency gives")]
        [DefaultValue(0.05f)]
        public float PotencyDamagePerPoint;
        [Tooltip("Determines the percent of Life Leech each point of Potency gives")]
        [DefaultValue(0.002)]
        [Range(0f, 0.1f)]
        [Increment(0.001f)]
        public float PotencyLeechPerPoint;
        [Tooltip("Determines the mana each point of Wits gives")]
        [DefaultValue(3)]
        public int WitsManaPerPoint;
        [Tooltip("Detemines the resistances each point of Wits gives")]
        [DefaultValue(1)]
        public int WitsResistancePerPoint;
    }
}
