using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Zenject;

namespace Gameplay.BattlePass
{
    public class
        BattlePassBonusViewModelFactory : IViewModelFactory<BattlePassBonusViewModel, BattlePassBonusView, BattlePassBonusModel>
    {
        private IStaticDataService _staticDataService;
        private BattlePassModel _battlePassModel;
        private IProgressService _progressService;

        [Inject]
        private void Construct(IStaticDataService staticDataService, BattlePassModel battlePassModel,
            IProgressService progressService)
        {
            _staticDataService = staticDataService;
            _battlePassModel = battlePassModel;
            _progressService = progressService;
        }

        public BattlePassBonusViewModel Create(BattlePassBonusModel model, BattlePassBonusView view)
        {
            return new BattlePassBonusViewModel(model, view, _staticDataService, _battlePassModel, _progressService);
        }
    }
}