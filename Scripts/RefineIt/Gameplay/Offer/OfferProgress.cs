using System;
using Gameplay.Offer;

namespace Gameplay.Offer
{
    [Serializable]
    public class OfferProgress
    {
        public OfferType StatusType;

        public OfferProgress()
        {
            StatusType = OfferType.StartOffer;
        }
    }
}