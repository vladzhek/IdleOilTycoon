using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Currencies;
using Gameplay.Services.Timer;
using Infrastructure.PersistenceProgress;
using Infrastructure.Purchasing;
using Infrastructure.StaticData;
using Infrastructure.Windows;

namespace Gameplay.BattlePass
{
    public class BattlePassModel
    {
        public event Action OnBuyBattlePass;
        public event Action BattlePassEnded;

        private readonly BattlePassQuestModel _battlePassQuestModel;
        private readonly CurrenciesModel _currenciesModel;
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly TimerService _timerService;
        private readonly BattlePassBonusModel _battlePassBonusModel;
        private readonly IWindowService _windowService;
        private readonly BattlePassEndSeasonModel _battlePassEndSeasonModel;
        private readonly IGenericPurchasingService _purchasingService;

        private BattlePassProgressData _progress;
        private BattlePassConfig _config;

        public TimeModel Timer { get; private set; }

        public BattlePassModel(BattlePassQuestModel battlePassQuestModel, CurrenciesModel currenciesModel,
            IProgressService progressService, IStaticDataService staticDataService, TimerService timerService,
            BattlePassBonusModel battlePassBonusModel, IWindowService windowService,
            BattlePassEndSeasonModel battlePassEndSeasonModel, IGenericPurchasingService purchasingService)
        {
            _battlePassQuestModel = battlePassQuestModel;
            _currenciesModel = currenciesModel;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _timerService = timerService;
            _battlePassBonusModel = battlePassBonusModel;
            _windowService = windowService;
            _battlePassEndSeasonModel = battlePassEndSeasonModel;
            _purchasingService = purchasingService;
        }

        public async void Initialize()
        {
            _progress = _progressService.PlayerProgress.BattlePassProgressData;
            _config = _staticDataService.BattlePassConfig;

            _battlePassQuestModel.Initialize();
            _battlePassBonusModel.Initialize();
            _battlePassEndSeasonModel.Initialize(_progress, this);

            InitializeRemainingTimeBattlePass();
            CreateBattlePassRewards(_config.FreeRewards, _progress.FreeRewards);
            CreateBattlePassRewards(_config.PremiumRewards, _progress.PremiumRewards);
        }

        public async void BuyBattlePass()
        {
            if (_progress.IsBuy) return;

            _progress.IsBuy = true;

            _battlePassBonusModel.Initialize();

            OnBuyBattlePass?.Invoke();
        }

        public void TakeReward(List<CurrencyData> rewardData, bool isFreeReward, int level)
        {
            var progress = _progressService.PlayerProgress.BattlePassProgressData;
            var rewardProgress = isFreeReward ? progress.FreeRewards : progress.PremiumRewards;

            if (rewardProgress[level].IsTakeRewards) return;

            foreach (var reward in rewardData)
            {
                _currenciesModel.Add(reward.Type, reward.Amount);
            }


            rewardProgress[level].IsTakeRewards = true;
        }

        private async void InitializeRemainingTimeBattlePass()
        {
            var firstDay = DateTime.Parse(_progressService.PlayerProgress.BattlePassFirstDay);
            var daysOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            bool isEndedSeason = DateTime.Now.DayOfYear - firstDay.DayOfYear > daysOfMonth;

            if (isEndedSeason)
            {
                await _windowService.Open(WindowType.BattlePassEndedSeason);

                _progress = new BattlePassProgressData();
                _progressService.PlayerProgress.BattlePassFirstDay = DateTime.Now.ToString();
            }

            var time = (int)(firstDay.AddDays(daysOfMonth) - DateTime.Now).TotalSeconds;
            CreateTimer(time);
        }

        private void CreateBattlePassRewards(BattlePassRewardConfig config, List<BattlePassRewardProgress> progresses)
        {
            if (progresses.Count == 0)
            {
                for (int index = 0; index < config.Rewards.Count; index++)
                {
                    progresses.Add(new BattlePassRewardProgress());
                }
            }
        }

        private void CreateTimer(int time)
        {
            Timer = _timerService.CreateTimer(TimerType.BattlePass.ToString(), time);
            Timer.Stopped += OnStopped;
        }

        private void OnStopped(TimeModel timer)
        {
            if (timer.TimeProgress.Time <= 0)
            {
                Initialize();
                BattlePassEnded?.Invoke();
            }
        }
    }
}