using System;
using UnityEngine;

namespace Gameplay.BattlePass
{
    [Serializable]
    public class BattlePassBonusData
    {
        public BattlePassBonusType Type;
        public Sprite BonusSprite;
        public int Value;
        public string Description;
    }
}