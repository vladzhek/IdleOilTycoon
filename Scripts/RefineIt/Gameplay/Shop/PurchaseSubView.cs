using System;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Shop
{
    public class PurchaseSubView : SubView<PurchaseViewData>
    {
        public event Action Click;
        public event Action<PurchaseViewData> InfoClick;

        [SerializeField] private Image _purchaseImage;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private Image _background;
        [SerializeField] private Image _counterBackground;
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private TextMeshProUGUI _productValue;
        [SerializeField] private TextMeshProUGUI _productName;

        [SerializeField] private Button _infoButton;

        private PurchaseViewData _data;

        public override void Initialize(PurchaseViewData data)
        {
            _data = data;

            SetProductValue(data);

            _purchaseImage.sprite = data.PurchaseSprite;
            _currencyImage.sprite = data.CurrencySprite;
            _priceValue.text = data.PriceString;
            _productName.text = data.PurchaseProductData.Name;
            _currencyImage.gameObject.SetActive(!data.PurchaseProductData.PurchaseData.IsInApp);
            _infoButton.gameObject.SetActive(data.PurchaseProductData.PurchaseData.Currencies.Count > 1);
            _background.sprite = data.Background;

            if (data.PurchaseProductData.PurchaseData.Currencies.Count <= 1)
            {
                _counterBackground.sprite = data.CounterBackground;
            }

            _infoButton.onClick.AddListener(InfoClickButton);
        }

        private void OnDisable()
        {
            _infoButton.onClick.RemoveListener(InfoClickButton);
        }

        private void SetProductValue(PurchaseViewData viewData)
        {
            if (viewData.PurchaseProductData.ProductType != ProductType.Set)
            {
                _productValue.text = viewData.PurchaseProductData.PurchaseData.Currencies[0].Amount.ToString();
            }
        }

        private void BuyClick()
        {
            Click?.Invoke();
        }

        private void InfoClickButton()
        {
            InfoClick?.Invoke(_data);
        }
    }
}