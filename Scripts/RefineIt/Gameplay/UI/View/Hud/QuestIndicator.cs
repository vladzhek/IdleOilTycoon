using System;
using Gameplay.Quests;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.View.Hud
{
    public class QuestIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject _indicator;
        
        private IQuestModel _questModel;

        [Inject]
        public void Construct(IQuestModel questModel)
        {
            _questModel = questModel;
        }

        private void OnEnable()
        {
            CheckAvailableRewards();
            
            _questModel.TakeReward += OnTakeReward;
        }

        private void OnDisable()
        {
            _questModel.TakeReward -= OnTakeReward;
        }

        private void OnTakeReward()
        {
            CheckAvailableRewards();
        }

        private void CheckAvailableRewards()
        {
           _indicator.SetActive(_questModel.isAvailableRewards());
        }
    }
}