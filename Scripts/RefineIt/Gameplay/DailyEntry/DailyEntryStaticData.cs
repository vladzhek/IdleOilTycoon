﻿using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Workspaces.Workers.Transport;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.DailyEntry
{
    [CreateAssetMenu(fileName = "DailyEntryData", menuName = "Data/DailyEntryData")]
    public class DailyEntryStaticData : ScriptableObject
    {
        public List<DailyEntryComponentData> DailyEntry;
        public Sprite CoinSprite;
        public Sprite DollarSprite;
        public CurrencyType SecondRewardTypeDay7;
        public int SecondRewardDay7;
    }
}