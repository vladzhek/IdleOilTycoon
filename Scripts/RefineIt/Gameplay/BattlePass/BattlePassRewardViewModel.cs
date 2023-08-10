using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Currencies;
using Gameplay.RewardPopUp;
using Infrastructure.AssetManagement;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Utils.Extensions;

namespace Gameplay.BattlePass
{
    public class BattlePassRewardViewModel : ViewModelBase<BattlePassModel, BattlePassView>
    {
        private readonly BattlePassProgressData _progress;
        private readonly BattlePassConfig _config;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly RewardPopupModel _rewardPopupModel;

        public BattlePassRewardViewModel(BattlePassModel model, BattlePassView view,
            IProgressService progressService, IStaticDataService staticDataService,
            IAssetProvider assetProvider, RewardPopupModel rewardPopupModel) : base(model, view)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _rewardPopupModel = rewardPopupModel;
            _progress = progressService.PlayerProgress.BattlePassProgressData;
            _config = staticDataService.BattlePassConfig;
        }

        public override async Task Show()
        {
            View.SetLevel(_progress.Level);
            View.SetExperienceSlider(_config.ExperienceForLevel, _progress.Experience);
            await InitializeRewardSubViews(View.RewardSubViewContainer, _config);
            SetLockStageReward();
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Model.Timer.Tick += OnTimerTick;
            Model.OnBuyBattlePass += BuyBattlePass;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            Model.Timer.Tick -= OnTimerTick;
            Model.OnBuyBattlePass += BuyBattlePass;
        }

        private void OnTimerTick(int time)
        {
            View.SetTimer(FormatTime.DayAndHoursToString(time));
        }

        private async Task InitializeRewardSubViews(BattlePassRewardSubViewContainer container,
            BattlePassConfig config)
        {
            container.CleanUp();
            for (int index = 0; index < config.FreeRewards.Rewards.Count; index++)
            {
                var premiumRewardConfig = config.PremiumRewards.Rewards[index];
                var freeRewardConfig = config.FreeRewards.Rewards[index];
                var premiumRewardProgress = _progress.PremiumRewards[index];
                var freeRewardProgress = _progress.FreeRewards[index];

                var data = new BattlePassStageRewardSubViewData
                {
                    FreeReward = await FillRewardData(freeRewardConfig, freeRewardProgress, true),
                    PremiumReward = await FillRewardData(premiumRewardConfig, premiumRewardProgress, false),
                    Level = index,
                    IsBuyBattlePass = _progress.IsBuy,
                    IsOpen = index < _progress.Level
                };

                container.Add(index.ToString(), data);
                container.SubViews[index.ToString()].TakeReward += OnTakeReward;
                container.SubViews[index.ToString()].RewardInfoClick += OnRewardInfoClick;
            }
        }

        private void OnRewardInfoClick(BattlePassRewardSubViewData data)
        {
            var rewardPopupData = new RewardsPopupData
            {
                LootBoxSprite = data.RewardSprite
            };
            
            foreach (var rewardData in data.RewardsData)
            {
                rewardPopupData.Rewards.Add(new RewardData
                {
                    RewardSprite = rewardData.CurrencySprite,
                    RewardQuantity = rewardData.CurrencyData.Amount
                });
            }
            
            _rewardPopupModel.ShowRewardPopUp(rewardPopupData);
        }

        private async void BuyBattlePass()
        {
            await InitializeRewardSubViews(View.RewardSubViewContainer, _config);

            for (int i = 0; i < _progress.Level; i++)
            {
                View.RewardSubViewContainer.SubViews[i.ToString()].SetOpen(true);
            }
        }

        private void OnTakeReward(BattlePassRewardSubViewData rewardData, int level)
        {
            var config = rewardData.isFreeReward ? _config.FreeRewards : _config.PremiumRewards;

            Model.TakeReward(config.Rewards[level].CurrenciesData, rewardData.isFreeReward, level);
        }

        private void SetLockStageReward()
        {
            for (int i = 0; i < _config.FreeRewards.Rewards.Count; i++)
            {
                View.RewardSubViewContainer.SubViews[i.ToString()].SetOpen(true);

                if (i >= _progress.Level)
                {
                    View.RewardSubViewContainer.SubViews[i.ToString()].SetOpen(false);
                    return;
                }
            }
        }

        private async Task<BattlePassRewardSubViewData> FillRewardData(BattlePassRewardData config,
            BattlePassRewardProgress progress, bool isFreeReward)
        {
            var data = new BattlePassRewardSubViewData
            {
                RewardQuantity = config.CurrenciesData.Count > 0 ? config.CurrenciesData[0].Amount : 0,
                IsTakeReward = progress.IsTakeRewards,
                isFreeReward = isFreeReward
            };

            if (config.Sprite.AssetGUID != "")
            {
                data.RewardSprite = await _assetProvider.LoadSprite(config.Sprite);
            }

            if (config.CurrenciesData.Count > 0)
            {
                foreach (CurrencyData currencyData in config.CurrenciesData)
                {
                    data.RewardsData.Add(new BattlePassRewardsInfoData
                    {
                        CurrencyData = currencyData,
                        CurrencySprite =
                            await _assetProvider.LoadSprite(
                                _staticDataService.GetCurrencyData(currencyData.Type).Sprite)
                    });
                }
            }

            return data;
        }
    }
}