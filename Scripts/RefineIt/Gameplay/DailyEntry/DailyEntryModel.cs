using System;
using System.Linq;
using Gameplay.Currencies;
using Gameplay.Services.Timer;
using Gameplay.TutorialStateMachine;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using Utils.Extensions;

namespace Gameplay.DailyEntry
{
    public class DailyEntryModel : IDailyEntryModel
    {
        public event Action OnTakeReward; 

        private readonly IWindowService _windowService;
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        private readonly CurrenciesModel _currenciesModel;
        private readonly TimerService _timerService;
        private readonly TutorialModel _tutorialModel;

        DailyEntryModel(IWindowService windowService, IStaticDataService staticDataService,
            IProgressService progressService, CurrenciesModel currenciesModel, TimerService timerService,
            TutorialModel tutorialModel)
        {
            _windowService = windowService;
            _staticDataService = staticDataService;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
            _timerService = timerService;
            _tutorialModel = tutorialModel;
        }

        public void Initializer()
        {
            InitializeCurrentDay();
            InitializeProgressDaily();
            InitializeTimer();

            if (_tutorialModel.StageType == TutorialStageType.Completed)
            {
                // _windowService.Open(WindowType.DailyEntry);
            }
        }

        public DailyEntryType GetCurDailyEntryType()
        {
            DailyEntryType day = (DailyEntryType)Enum.Parse(typeof(DailyEntryType),
                _progressService.PlayerProgress.CurrentDailyEntry.ToString());
            return day;
        }

        public DailyEntryComponentData GetComponentData(DailyEntryType day)
        {
            return _staticDataService.DailyEntryData.DailyEntry.Find(x => x.Day == day);
        }

        public DailyEntryStaticData GetStaticData()
        {
            return _staticDataService.DailyEntryData;
        }

        public DailyEntryProgress GetProgress(DailyEntryType day)
        {
            return _progressService.PlayerProgress.GetOrCreateDailyEntry(day);
        }

        public void TakeReward(DailyEntryType day, CurrencyType currencyType, int reward)
        {
            var progress = _progressService.PlayerProgress.DailyEntryProgresses.Find(x
                => x.Day == day);
            progress.IsTake = true;
            progress.IsVisableReward = false;
            _currenciesModel.Add(currencyType, reward);

            if (day == DailyEntryType.Day7)
            {
                var type = _staticDataService.DailyEntryData.SecondRewardTypeDay7;
                var secondReward = _staticDataService.DailyEntryData.SecondRewardDay7;
                _currenciesModel.Add(type, secondReward);
            }
            
            OnTakeReward?.Invoke();
        }

        private void InitializeCurrentDay()
        {
            DateTime lastSession = DateTime.MinValue;
            DateTime minValue = DateTime.MinValue;

            if (_progressService.PlayerProgress.LastSession != null)
            {
                lastSession = DateTime.Parse(_progressService.PlayerProgress.LastSession);
            }

            var dateNow = DateTime.Now;
            lastSession = lastSession.Date.AddHours(minValue.Hour);
            dateNow = dateNow.Date.AddHours(minValue.Hour);

            if ((dateNow - lastSession).TotalDays >= 1)
            {
                if (_progressService.PlayerProgress.CurrentDailyEntry < 7)
                    _progressService.PlayerProgress.CurrentDailyEntry++;
                else
                {
                    _progressService.PlayerProgress.CurrentDailyEntry = 1;
                    _progressService.PlayerProgress.ReCreateProgress();
                }
            }
        }

        private void InitializeProgressDaily()
        {
            foreach (var progress in _progressService.PlayerProgress.DailyEntryProgresses)
            {
                _progressService.PlayerProgress.GetOrCreateDailyEntry(progress.Day);
                if (progress.Day == GetCurDailyEntryType() && !progress.IsTake)
                {
                    progress.IsVisableReward = true;
                }

                if (progress.Day < GetCurDailyEntryType() && !progress.IsTake)
                {
                    var component = GetComponentData(progress.Day);
                    TakeReward(progress.Day, component.rewardType, int.Parse(component.reward));
                }
            }
        }

        public bool IsAvailableRewards()
        {
            return _progressService.PlayerProgress.DailyEntryProgresses
                .Select(progress => progress.Day == GetCurDailyEntryType() && !progress.IsTake).FirstOrDefault();
        }

        private void InitializeTimer()
        {
            _timerService.TimeModels[TimerType.DailyTimer.ToString()].Stopped += StoppedDailyTimer;
            int time = FormatTime.HoursIntFormat(DateTime.Now.Hour)
                       + FormatTime.MinutesIntFormat(DateTime.Now.Minute)
                       + DateTime.Now.Second;
            _timerService.TimeModels[TimerType.DailyTimer.ToString()].TimeProgress.Time = 86400;
            _timerService.TimeModels[TimerType.DailyTimer.ToString()].TimeProgress.Time -= time;
        }

        private void StoppedDailyTimer(TimeModel timer)
        {
            _windowService.Close(WindowType.DailyEntry);
            if (_progressService.PlayerProgress.CurrentDailyEntry < 7)
                _progressService.PlayerProgress.CurrentDailyEntry++;
            else
            {
                _progressService.PlayerProgress.CurrentDailyEntry = 1;
                _progressService.PlayerProgress.ReCreateProgress();
            }

            InitializeProgressDaily();
            _windowService.Open(WindowType.DailyEntry);
        }
    }
}