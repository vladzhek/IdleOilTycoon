using Gameplay.Offer.UI;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;

namespace Gameplay.Shop
{
    public class ShopView : MonoBehaviour
    {
        [field: SerializeField]
        public SubViewContainer<PurchaseSubView, PurchaseViewData> SetProductSubViewContainer { get; private set; }

        [field: SerializeField]
        public SubViewContainer<PurchaseSubView, PurchaseViewData> CurrenciesProductSubViewContainer { get; private set; }

        [SerializeField] public OfferPrefab OfferPrefab;
    }
}