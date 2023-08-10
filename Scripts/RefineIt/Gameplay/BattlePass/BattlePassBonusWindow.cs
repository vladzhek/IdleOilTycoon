using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.BattlePass
{
    public class BattlePassBonusWindow : Window
    {
        [SerializeField] private BattlePassBonusViewInitializer battlePassBonusViewInitializer;

        private BattlePassBonusModel _battlePassBonusModel;

        [Inject]
        public void Construct(BattlePassBonusModel battlePassBonusModel)
        {
            _battlePassBonusModel = battlePassBonusModel;
        }

        private void OnEnable()
        {
            battlePassBonusViewInitializer.Initialize(_battlePassBonusModel);
        }
    }
}