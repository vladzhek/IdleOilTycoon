using Infrastructure.Windows.MVVM;

namespace Gameplay.BattlePass
{
    public class BattlePassQuestViewModelFactory : IViewModelFactory<BattlePassQuestViewModel, BattlePassQuestView, BattlePassQuestModel>
    {
        private readonly BattlePassModel _battlePassModel;

        public BattlePassQuestViewModelFactory(BattlePassModel battlePassQuestModel)
        {
            _battlePassModel = battlePassQuestModel;
        }

        public BattlePassQuestViewModel Create(BattlePassQuestModel model, BattlePassQuestView questView)
        {
            return new BattlePassQuestViewModel(model, questView, _battlePassModel);
        }
    }
}