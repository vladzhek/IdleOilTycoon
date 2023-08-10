using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.BattlePass
{
    public class BattlePassView : MonoBehaviour
    {
        [SerializeField] private Slider _experienceSlider;
        [SerializeField] private TextMeshProUGUI _experienceValue;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _timer;
        
        public BattlePassRewardSubViewContainer RewardSubViewContainer;

        public void SetExperienceSlider(int maxValue, int currentValue)
        {
            _experienceSlider.value = (float)currentValue / maxValue;
            _experienceValue.text = $"{currentValue}/{maxValue}";
        }

        public void SetLevel(int level)
        {
            _level.text = $"{level + 1}";
        }

        public void SetTimer(string timer)
        {
            _timer.text = timer;
        }
    }
}