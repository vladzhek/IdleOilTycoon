using System;
using Gameplay.Currencies;

namespace Gameplay.Offer
{
    [Serializable]
    public class OfferReward
    {
        public CurrencyType CurrencyType;
        public float Amount;
    }
}