using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.BattlePass
{
    public class BattlePassQuestWindow : Window
    {
        [SerializeField] private BattlePassQuestViewInitializer _battlePassQuestViewInitializer;

        private BattlePassQuestModel _battlePassQuestModel;

        [Inject]
        public void Construct(BattlePassQuestModel battlePassQuestModel)
        {
            _battlePassQuestModel = battlePassQuestModel;
        }

        private void OnEnable()
        {
            _battlePassQuestViewInitializer.Initialize(_battlePassQuestModel);
        }
    }
}