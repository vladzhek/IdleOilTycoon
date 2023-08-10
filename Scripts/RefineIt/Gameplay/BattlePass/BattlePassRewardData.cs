using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using UnityEngine.AddressableAssets;

namespace Gameplay.BattlePass
{
    [Serializable]
    public class BattlePassRewardData
    {
        public AssetReferenceSprite Sprite;
        public List<CurrencyData> CurrenciesData = new();
    }
}