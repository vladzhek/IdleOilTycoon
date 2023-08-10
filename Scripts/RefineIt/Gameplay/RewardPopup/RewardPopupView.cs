using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.RewardPopUp
{
    public class RewardPopupView : MonoBehaviour
    {
        [SerializeField] private Image _lootBoxImage;

        public RewardPopupSubViewContainer RewardPopupSubViewContainer;

        public void SetLootBoxSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                _lootBoxImage.gameObject.SetActive(false);
                return;
            }

            _lootBoxImage.gameObject.SetActive(true);
            _lootBoxImage.sprite = sprite;
        }
    }
}