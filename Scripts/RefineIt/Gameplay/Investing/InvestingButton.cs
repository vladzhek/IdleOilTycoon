using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Investing
{
    public class InvestingButton : MonoBehaviour
    {
        [SerializeField] private GameObject _adsBG;
        [SerializeField] private GameObject _icon;
        [SerializeField] private GameObject _disableIcon;
        [SerializeField] private TMP_Text _text;

        public void IconActive(bool isActive)
        {
            _icon.SetActive(isActive);
        }

        public void SetText(string text)
        {
            _text.text = text;
        }
        
        public void SetIcon(Sprite sprite)
        {
            _icon.GetComponent<Image>().sprite = sprite;
        }

        public void IsActiveAdsBG(bool isActive)
        {
            _adsBG.SetActive(isActive);
        }
        
        public void IsActiveDisableIcon(bool isActive)
        {
            _disableIcon.SetActive(isActive);
        }
    }
}