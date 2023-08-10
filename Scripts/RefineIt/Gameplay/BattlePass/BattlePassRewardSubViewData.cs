using System.Collections.Generic;
using Gameplay.Currencies;
using UnityEngine;

namespace Gameplay.BattlePass
{
    public class BattlePassRewardSubViewData
    {
        public Sprite RewardSprite;
        public int RewardQuantity;
        public BattlePassRewardType RewardType;
        public bool IsTakeReward;
        public bool isFreeReward;
        public List<BattlePassRewardsInfoData> RewardsData = new();
    }

    public class BattlePassRewardsInfoData
    {
        public Sprite CurrencySprite;
        public CurrencyData CurrencyData;
    }
}