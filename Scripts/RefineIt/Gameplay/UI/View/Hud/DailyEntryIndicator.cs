using System;
using Gameplay.DailyEntry;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.View.Hud
{
    public class DailyEntryIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject _indicator;

        private IDailyEntryModel _dailyEntryModel;

        [Inject]
        public void Construct(IDailyEntryModel dailyEntryModel)
        {
            _dailyEntryModel = dailyEntryModel;
        }

        private void OnEnable()
        {
            CheckAvailableReward();

            _dailyEntryModel.OnTakeReward += TakeRewarded;
        }

        private void OnDisable()
        {
            _dailyEntryModel.OnTakeReward -= TakeRewarded;
        }

        private void TakeRewarded()
        {
            CheckAvailableReward();
        }

        private void CheckAvailableReward()
        {
            _indicator.SetActive(_dailyEntryModel.IsAvailableRewards());
        }
    }
}