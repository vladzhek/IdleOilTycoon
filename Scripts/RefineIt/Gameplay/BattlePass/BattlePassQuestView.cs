using System;
using Gameplay.Region.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.BattlePass
{
    public class BattlePassQuestView : MonoBehaviour
    {
        [SerializeField] private Slider _experienceSlider;
        [SerializeField] private TextMeshProUGUI _experienceValue;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _battlePassTimer;
        [SerializeField] private TextMeshProUGUI _dailyTimer;
        [SerializeField] private TextMeshProUGUI _weeklyTimer;

        [field:SerializeField] public BattlePassQuestContainer DailyQuestContainer{ get; private set; }
        [field:SerializeField] public BattlePassQuestContainer WeeklyQuestContainer{ get; private set; }
        
        public void SetExperienceSlider(int maxValue, int currentValue)
        {
            _experienceSlider.value = (float)currentValue / maxValue;
            _experienceValue.text = $"{currentValue}/{maxValue}";
        }

        public void SetLevel(int level)
        {
            _level.text = $"{level + 1}";
        }

        public void SetBattlePassTimer(string timer)
        {
            _battlePassTimer.text = timer;
        }
        
        public void SetDailyTimer(string timer)
        {
            _dailyTimer.text = timer;
        }
        
        public void SetWeeklyTimer(string timer)
        {
            _weeklyTimer.text = timer;
        }
    }
}