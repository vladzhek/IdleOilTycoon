using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.RewardPopUp
{
    [Serializable]
    public class RewardsPopupData
    {
        public List<RewardData> Rewards = new();

        public Sprite LootBoxSprite;
    }
}