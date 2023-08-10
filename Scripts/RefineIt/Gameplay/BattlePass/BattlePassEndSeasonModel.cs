using System.Threading.Tasks;
using Gameplay.RewardPopUp;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows;

namespace Gameplay.BattlePass
{
    public class BattlePassEndSeasonModel
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IWindowService _windowService;
        private readonly RewardPopupModel _rewardPopupModel;
        private readonly IAssetProvider _assetProvider;

        private BattlePassModel _battlePassModel;

        private BattlePassProgressData _progress = new();

        public BattlePassEndSeasonModel(IStaticDataService staticDataService, IWindowService windowService,
            RewardPopupModel rewardPopupModel, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _windowService = windowService;
            _rewardPopupModel = rewardPopupModel;
            _assetProvider = assetProvider;
        }

        private BattlePassConfig _config => _staticDataService.BattlePassConfig;

        public void Initialize(BattlePassProgressData progressData, BattlePassModel battlePassModel)
        {
            _progress = progressData;
            _battlePassModel = battlePassModel;
        }

        public async void GetRewards()
        {
            var rewardPopupData = new RewardsPopupData();

            for (int i = 0; i < _progress.Level; i++)
            {
                if (_progress.IsBuy)
                {
                    var rewardConfig = _config.PremiumRewards.Rewards[i];
                    _battlePassModel.TakeReward(rewardConfig.CurrenciesData, false, i);
                    await CreateRewardPopUp(rewardConfig, rewardPopupData);
                }
                

                var freeRewardConfig = _config.FreeRewards.Rewards[i];

                await CreateRewardPopUp(freeRewardConfig, rewardPopupData);

                _battlePassModel.TakeReward(freeRewardConfig.CurrenciesData, false, i);
            }

            if (rewardPopupData.Rewards.Count > 0)
            {
                _rewardPopupModel.ShowRewardPopUp(rewardPopupData);
            }

            _windowService.Close(WindowType.BattlePassEndedSeason);
        }

        private async Task CreateRewardPopUp(BattlePassRewardData rewardConfig, RewardsPopupData rewardPopupData)
        {
            foreach (var data in rewardConfig.CurrenciesData)
            {
                var currencySprite = await _assetProvider
                    .LoadSprite(_staticDataService.GetCurrencyData(data.Type).Sprite);

                var rewardData = rewardPopupData.Rewards.Find(x => x.RewardSprite == currencySprite);

                if (rewardData != null)
                {
                    rewardData.RewardQuantity += data.Amount;
                }
                else
                {
                    rewardPopupData.Rewards.Add(new RewardData
                    {
                        RewardSprite = currencySprite,
                        RewardQuantity = data.Amount
                    });
                }
            }
        }
    }
}