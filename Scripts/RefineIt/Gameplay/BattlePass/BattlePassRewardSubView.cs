using System;
using DG.Tweening;
using Gameplay.RewardPopUp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.BattlePass
{
    public class BattlePassRewardSubView : MonoBehaviour
    {
        public event Action<BattlePassRewardSubViewData> TakeReward;
        public event Action<BattlePassRewardSubViewData> RewardsInfoClick; 

        [SerializeField] private TextMeshProUGUI _rewardQuantity;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private GameObject _lock;
        [SerializeField] private Button _takeRewardButton;
        [SerializeField] private Button _rewardsInfoButton;

        private BattlePassRewardSubViewData _data;

        public void Initialize(BattlePassRewardSubViewData data, bool isBuyBattlePass, bool isFreeReward, bool isOpen)
        {
            _data = data;
            
            if (data.RewardSprite == null)
            {
                gameObject.GetComponent<Image>().color = Color.clear;
                gameObject.GetComponent<Mask>().enabled = true;
                return;
            }
            
            _takeRewardButton.onClick.AddListener(TakeRewardClick);
            _rewardsInfoButton.onClick.AddListener(ShowInfoClick);

            CheckAvailableBButton(isBuyBattlePass, isFreeReward, isOpen, data.IsTakeReward, data.RewardsData.Count > 0);
            
            _rewardIcon.sprite = data.RewardSprite;
            _rewardQuantity.text = $"{data.RewardQuantity}";
            _rewardsInfoButton.gameObject.SetActive(data.RewardsData.Count > 1);
            _rewardQuantity.transform.parent.gameObject.SetActive(data.RewardsData.Count == 1);
        }

        private void CheckAvailableBButton(bool isBuyBattlePass, bool isFreeReward,
            bool isOpen, bool isTakeReward, bool isNotEmpty)
        {
            if (isFreeReward)
            {
                _takeRewardButton.gameObject.SetActive(isOpen && !isTakeReward && isNotEmpty);
            }
            else
            {
                _takeRewardButton.gameObject.SetActive(isOpen && isBuyBattlePass && !isTakeReward && isNotEmpty);
                _lock.SetActive(!isBuyBattlePass);
            }
        }

        private void TakeRewardClick()
        {
            TakeReward?.Invoke(_data);
            _takeRewardButton.gameObject.SetActive(false);
        }
        
        private void ShowInfoClick()
        {
            RewardsInfoClick?.Invoke(_data);
        }
    }
}