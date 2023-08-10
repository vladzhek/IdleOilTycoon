using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.BattlePass
{
    public class BattlePassEndSeasonWindow : Window
    {
        [SerializeField] private BattlePassEndSeasonViewInitializer _battlePassEndSeasonViewInitializer;

        private BattlePassEndSeasonModel _battlePassModel;

        [Inject]
        public void Construct(BattlePassEndSeasonModel battlePassModel)
        {
            _battlePassModel = battlePassModel;
        }

        public void OnEnable()
        {
            _battlePassEndSeasonViewInitializer.Initialize(_battlePassModel);
        }
    }
}