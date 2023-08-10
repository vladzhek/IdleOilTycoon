using System;
using UnityEngine;

namespace Gameplay.BattlePass
{
    [Serializable]
    public class BattlePassBonusSubViewData
    {
        public BattlePassBonusType Type;
        public Sprite BonusSprite;
        public string Value;
        public string Description;
        public bool IsBuy;
    }
}