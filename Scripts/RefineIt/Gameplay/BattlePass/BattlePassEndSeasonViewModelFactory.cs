using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.BattlePass
{
    public class BattlePassEndSeasonViewModelFactory : IViewModelFactory<BattlePassEndSeasonViewModel,
        BattlePassEndSeasonView, BattlePassEndSeasonModel>
    {
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public BattlePassEndSeasonViewModelFactory(IProgressService progressService,
            IStaticDataService staticDataService)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
        }

        public BattlePassEndSeasonViewModel Create(BattlePassEndSeasonModel model, BattlePassEndSeasonView view)
        {
            return new BattlePassEndSeasonViewModel(model, view, _progressService, _staticDataService);
        }
    }
}