using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Offer.UI
{
    public class OfferView : MonoBehaviour
    {
        [SerializeField] private CurrencyBlock _currencyBlock;
        [SerializeField] private RectTransform _currencyContainer;
        [SerializeField] private Image _mainIcon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _buttonBuy;

        public void SetIcon(Sprite icon)
        {
            _mainIcon.sprite = icon;
        }

        public void SetTitle(string title)
        {
            _title.text = title;
        }

        public void SetDescription(string desc)
        {
            _description.text = desc;
        }

        public void SetTextPrice(string amount)
        {
            _priceText.text = amount;

            if (amount == "")
            {
                _priceText.gameObject.SetActive(false);
            }
        }

        public void SpawnCurrency(Sprite sprite, string amount)
        {
            var currencyBlock =
                Instantiate(_currencyBlock, _currencyContainer);
            currencyBlock.SetCurrency(sprite, amount);
        }

        public Button GetBuyButton()
        {
            return _buttonBuy;
        }

        public void ClearRewardsContainer()
        {
            foreach (Transform  child  in _currencyContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}