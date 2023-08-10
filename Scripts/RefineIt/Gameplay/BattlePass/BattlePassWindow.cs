using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.BattlePass
{
    public class BattlePassWindow : Window
    {
        [SerializeField] private BattlePassViewInitializer _battlePassViewInitializer;

        private BattlePassModel _battlePassModel;

        [Inject]
        public void Construct(BattlePassModel battlePassModel)
        {
            _battlePassModel = battlePassModel;
        }

        private void OnEnable()
        {
            _battlePassViewInitializer.Initialize(_battlePassModel);
        }
    }
}