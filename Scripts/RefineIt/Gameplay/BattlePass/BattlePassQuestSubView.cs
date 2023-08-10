using Gameplay.Orders;
using Gameplay.Region.Quests;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Gameplay.BattlePass
{
    public class BattlePassQuestSubView : SubView<QuestSubData>
    {
        [SerializeField] private Image _imageDaily;
        [SerializeField] private TMP_Text _textDescription;
        [SerializeField] private TMP_Text _textReward;
        [SerializeField] private TakeDailyRewardButton _buttonTake;
        [SerializeField] private Slider _questProgress;
        [SerializeField] private TMP_Text _questProgressText;
        [SerializeField] private GameObject _completeBG;

        public TakeDailyRewardButton ButtonTake => _buttonTake;

        public override void Initialize(QuestSubData data)
        {
            var sliderValue = (float)data.Progress / data.Count;
            
            _imageDaily.sprite = data.Sprite;
            _textDescription.text = data.Description;
            _textReward.text = data.Reward.ToFormattedBigNumber();
            _questProgress.value = sliderValue;
            _questProgressText.text = $"{data.Progress}/{data.Count}";
            _buttonTake.Initialize(data.Reward, data.Guid);

            HideRewardButton(data.IsTakeReward);
        }

        public void ActiveRewardButton(bool isActive)
        {
            _buttonTake.gameObject.GetComponent<Button>().interactable = isActive;
        }
        
        public void HideRewardButton(bool isTakeReward)
        {
            _buttonTake.transform.parent.gameObject.SetActive(!isTakeReward);
            _completeBG.SetActive(isTakeReward);
        }
    }
}