using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Offer.UI
{
    public class OfferPrefab : MonoBehaviour
    {
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticDataService;
        
        [SerializeField] private Image _icon;
        [SerializeField] private RectTransform _footerPanel;
        [SerializeField] private CurrencyBlock _currencyBlock;
        [SerializeField] private TMP_Text _priceText;

        [Inject]
        private void Construct(IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public void Initialize(Sprite icon, OfferData data, string price)
        {
            ClearReward();
            
            _icon.sprite = icon;
            _priceText.text = "$ " + price;
            foreach (var value in data.purchaseData.Currencies)
            {
                var sprite = _assetProvider.LoadSprite(_staticDataService.GetCurrencyData(value.Type).Sprite).Result;
                SpawnCurrency(sprite, value.Amount.ToString());
            }
        }

        private void SpawnCurrency(Sprite sprite, string amount)
        {
            var currencyBlock = 
                Instantiate(_currencyBlock, _footerPanel);
            currencyBlock.SetCurrency(sprite, amount);
        }

        private void ClearReward()
        {
            foreach (Transform child in _footerPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}