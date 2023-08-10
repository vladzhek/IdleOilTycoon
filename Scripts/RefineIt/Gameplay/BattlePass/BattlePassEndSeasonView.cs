using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.BattlePass
{
    public class BattlePassEndSeasonView : MonoBehaviour
    {
        public event Action GetRewards; 

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _experienceText;
        [SerializeField] private Button _getRewardsButton;

        public void SetData(int level, int experience)
        {
            _levelText.text = $"Ур.{level}";
            _experienceText.text = $"{experience}";
            
            _getRewardsButton.onClick.AddListener(GetRewardsClick);
        }

        private void OnDisable()
        {
            _getRewardsButton.onClick.RemoveListener(GetRewardsClick);
        }

        private void GetRewardsClick()
        {
            GetRewards?.Invoke();
        }
    }
}