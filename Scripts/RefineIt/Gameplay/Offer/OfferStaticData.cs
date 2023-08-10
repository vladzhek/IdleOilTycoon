using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Offer
{
    [CreateAssetMenu(fileName = "Offer", menuName = "Data/Offer")]
    public class OfferStaticData : ScriptableObject
    {
        public List<OfferData> Offers;
    }
}