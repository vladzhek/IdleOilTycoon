using System;
using Gameplay.Investing;
using Gameplay.Quests;

namespace Gameplay.Offer
{
    public interface IOfferModel
    {
        OfferProgress GetProgressData();
        void BuyOffer(string purchaseID);
        event Action OnBuyOffer;
        void Initialize();
    }
}