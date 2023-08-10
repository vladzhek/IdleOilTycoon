using Gameplay.RewardPopUp;
using Infrastructure.AssetManagement;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.BattlePass
{
    public class
        BattlePassRewardViewModelFactory : IViewModelFactory<BattlePassRewardViewModel, BattlePassView, BattlePassModel>
    {
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly RewardPopupModel _rewardPopupModel;

        public BattlePassRewardViewModelFactory(IProgressService progressService, IStaticDataService staticDataService,
            IAssetProvider assetProvider, RewardPopupModel rewardPopupModel)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _rewardPopupModel = rewardPopupModel;
        }

        public BattlePassRewardViewModel Create(BattlePassModel model, BattlePassView view)
        {
            return new BattlePassRewardViewModel(model, view, _progressService, _staticDataService, _assetProvider,
                _rewardPopupModel);
        }
    }
}