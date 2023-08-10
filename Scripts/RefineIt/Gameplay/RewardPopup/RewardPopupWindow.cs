using System;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.RewardPopUp
{
    public class RewardPopupWindow : Window
    {
        [SerializeField] private RewardPopupViewInitializer _viewInitializer;

        private RewardPopupModel _rewardPopupModel;

        [Inject]
        public void Construct(RewardPopupModel rewardPopupModel)
        {
            _rewardPopupModel = rewardPopupModel;
        }

        private void OnEnable()
        {
            _viewInitializer.Initialize(_rewardPopupModel);
        }
    }
}