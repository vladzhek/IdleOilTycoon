using System;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.DailyEntry.UI
{
    public class DailyEntrySubView : SubView<DailyEntrySubData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private DailyEntryTakeReward _buttonTake;
        [SerializeField] private GameObject _сheckGO;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _secondRewardText;
        

        public override void Initialize(DailyEntrySubData data)
        {
            _description.text = data.Description;
            _image.sprite = data.Sprite;
            _rewardText.text = data.Reward;
            _buttonTake.Initialize(data.Day,data.Currency,int.Parse(data.Reward));
            
            _buttonTake.gameObject.SetActive(data.IsShowRewardTake);
            _сheckGO.SetActive(data.IsTake);
            
            if (data.SecondReward != "" && _secondRewardText != null)
            {
                _secondRewardText.text = data.SecondReward;
            }
        }

        public DailyEntryTakeReward GetRewardButton()
        {
            return _buttonTake;
        }

        public void ActiveCheck(bool isActive)
        {
            _сheckGO.SetActive(isActive);
        }
    }
}