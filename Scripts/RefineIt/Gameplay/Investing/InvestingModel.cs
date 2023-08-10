using System;
using Gameplay.Currencies;
using Gameplay.Investing.UI;
using Gameplay.Quests;
using Gameplay.OfflineProgressService;
using Gameplay.Services.Timer;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.YandexAds;
using UnityEngine;
using Zenject;

namespace Gameplay.Investing
{
    public class InvestingModel : IInvestingModel
    {
        private const string INVEST_TIMER = "Investing";
        private IProgressService _progressService;
        private IStaticDataService _staticDataService;
        private TimerService _timerService;
        private CurrenciesModel _currenciesModel;
        private readonly IAdsService _yandexAds;
        
        public event Action<int> OnTimerTick;
        public event Action OnTimerStopped;
        
        InvestingModel(IProgressService progressService, 
            IStaticDataService staticDataService, 
            CurrenciesModel currenciesModel,
            IAdsService yandexAds, 
            TimerService timerService )
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _timerService = timerService;
            _currenciesModel = currenciesModel;
            _yandexAds = yandexAds;
        }

        public void Initialize()
        {
            var data = _progressService.PlayerProgress.InvestingProgresses;
            if (data.StatusType == ViewStatusType.InProgress)
            {
                DateTime lastSession = DateTime.Parse(_progressService.PlayerProgress.LastSession);
                TimeSpan timeDifference = DateTime.Now - lastSession;
                data.Timer -= (int)timeDifference.TotalSeconds;
                
                _timerService.CreateTimer(INVEST_TIMER, data.Timer);
                _timerService.TimeModels[INVEST_TIMER].Tick += TimerTick;
                _timerService.TimeModels[INVEST_TIMER].Stopped += TimerStopped;
            }
        }

        public void StartInvestingProcess()
        { 
            var data = _progressService.PlayerProgress.InvestingProgresses;
            var staticData = _staticDataService.InvestingData;
            if(_currenciesModel.Get(CurrencyType.HardCurrency) < staticData.InvestingHardCurrency) 
                return;
            
            _currenciesModel.Consume(CurrencyType.HardCurrency, staticData.InvestingHardCurrency);
            data.StatusType = ViewStatusType.InProgress;
            data.CountBoost = staticData.CountBoost;
            _timerService.CreateTimer(INVEST_TIMER, 14400); // 4 hour
            _timerService.TimeModels[INVEST_TIMER].Tick += TimerTick;
            _timerService.TimeModels[INVEST_TIMER].Stopped += TimerStopped;
        }

        private void TimerStopped(TimeModel timeObj)
        {
            _progressService.PlayerProgress.InvestingProgresses.StatusType = ViewStatusType.ReadyToTake;
            OnTimerStopped?.Invoke();
        }

        private void TimerTick(int time)
        {
            _progressService.PlayerProgress.InvestingProgresses.Timer = time;
            OnTimerTick?.Invoke(time);
        }

        public void BoostInvestingProcess()
        {
            if(!_timerService.TimeModels.ContainsKey(INVEST_TIMER))
                return;
            
            if(_progressService.PlayerProgress.InvestingProgresses.CountBoost <= 0)
                return;
            
            if(_yandexAds.ShowAdsVideo())
                _yandexAds.VideoShown += BoostAfterAds;
        }

        private void BoostAfterAds()
        {
            _progressService.PlayerProgress.InvestingProgresses.CountBoost -= 1;
            _timerService.TimeModels[INVEST_TIMER].TimeProgress.Time -= 600; // 10 minute
            _yandexAds.VideoShown -= BoostAfterAds;
        }
        
        public void GetInvestingReward()
        {
            _currenciesModel.Add(CurrencyType.SoftCurrency, GetAmountForReward());
            _progressService.PlayerProgress.InvestingProgresses.StatusType = ViewStatusType.Default;
        }

        public int GetAmountForReward()
        {
            //TODO: Переделать с расчётом под 1 хадра n софты * 1,40
            return 140000;
        }

        public InvestingProgress GetProgressData()
        {
            return _progressService.PlayerProgress.InvestingProgresses;
        }
        
        public InvestingStaticData GetStaticData()
        {
            return _staticDataService.InvestingData;
        }
    }
}