using System;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Shop
{
    public class CurrencyProductSubView : SubView<PurchaseViewData>
    {
        public event Action Click;

        [SerializeField] private Image _purchaseImage;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private TextMeshProUGUI _productValue;
        [SerializeField] private TextMeshProUGUI _productName;

        public override void Initialize(PurchaseViewData data)
        {
            SetProductValue(data);

            _purchaseImage.sprite = data.PurchaseSprite;
            _currencyImage.sprite = data.CurrencySprite;
            _priceValue.text = data.PurchaseProductData.PurchaseData.Price[0].Amount.ToString();
            _productName.text = data.PurchaseProductData.Name;
        }

        private void SetProductValue(PurchaseViewData viewData)
        {
            if (viewData.PurchaseProductData.ProductType != ProductType.Set)
            {
                _productValue.text = viewData.PurchaseProductData.PurchaseData.Price[0].Amount.ToString();;
            }
        }

        public void BuyClick()
        {
            Click?.Invoke();
        }
    }
}