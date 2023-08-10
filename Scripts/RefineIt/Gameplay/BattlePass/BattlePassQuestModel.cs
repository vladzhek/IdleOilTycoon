using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Quests;
using Gameplay.Services.Timer;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Random = UnityEngine.Random;

namespace Gameplay.BattlePass
{
    public class BattlePassQuestModel
    {
        public event Action<QuestModel> UpdateDailyQuest;
        public event Action<QuestModel> UpdateWeeklyQuest;
        public event Action<bool> RecreatedDailyQuests;

        private const int AMOUNT_DAILY_QUESTS = 4;
        private const int AMOUNT_WEEKLY_QUESTS = 7;

        private readonly IProgressService _progressService;
        private readonly IQuestFactory _questFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly TimerService _timerService;

        private QuestStaticData _dailyQuestsConfig;
        private QuestStaticData _weeklyQuestsConfig;
        private BattlePassProgressData _progress;

        public TimeModel DailyTimer { get; private set; }
        public TimeModel WeeklyTimer { get; private set; }

        public readonly Dictionary<QuestsGuid, QuestModel> DailyQuestModels = new();
        public readonly Dictionary<QuestsGuid, QuestModel> WeeklyQuestModels = new();

        public BattlePassQuestModel(IStaticDataService staticDataService, IQuestFactory questFactory,
            IProgressService progressService, TimerService timerService)
        {
            _staticDataService = staticDataService;
            _questFactory = questFactory;
            _progressService = progressService;
            _timerService = timerService;
        }

        public BattlePassProgressData Progress => _progressService.PlayerProgress.BattlePassProgressData;
        public BattlePassConfig BattlePassConfig => _staticDataService.BattlePassConfig;

        public void Initialize()
        {
            _progress = _progressService.PlayerProgress.BattlePassProgressData;
            _dailyQuestsConfig = _staticDataService.DailyQuestData;
            _weeklyQuestsConfig = _staticDataService.WeeklyQuestData;

            InitializeQuests();
        }

        private void InitializeQuests()
        {
            if (_progressService.PlayerProgress.LastSession == null)
            {
                CreateQuests(_dailyQuestsConfig, _progress.DailyQuestProgress, DailyQuestModels, true,
                    AMOUNT_DAILY_QUESTS, TimerType.BattlePassDailyTimer);

                CreateQuests(_weeklyQuestsConfig, _progress.WeeklyQuestProgress, WeeklyQuestModels, true,
                    AMOUNT_WEEKLY_QUESTS, TimerType.BattlePassWeeklyTimer);
                return;
            }

            var dateCreateWeekQuests = _progressService.PlayerProgress.DateCreateWeekQuests != ""
                ? DateTime.Parse(_progressService.PlayerProgress.DateCreateWeekQuests)
                : DateTime.Now;

            var lastSession = DateTime.Parse(_progressService.PlayerProgress.LastSession);

            bool isNewDay = lastSession.DayOfYear < DateTime.Now.DayOfYear;
            bool isNewWeek = DateTime.Now.DayOfYear - dateCreateWeekQuests.DayOfYear >= 7;

            CreateQuests(_weeklyQuestsConfig, _progress.WeeklyQuestProgress, WeeklyQuestModels, isNewWeek,
                AMOUNT_WEEKLY_QUESTS, TimerType.BattlePassWeeklyTimer);
            CreateQuests(_dailyQuestsConfig, _progress.DailyQuestProgress, DailyQuestModels, isNewDay,
                AMOUNT_DAILY_QUESTS, TimerType.BattlePassDailyTimer);
        }

        private void CreateQuests(QuestStaticData config, List<QuestsProgress> progresses,
            Dictionary<QuestsGuid, QuestModel> models, bool NewQuests, int quantityQuests, TimerType timerType)
        {
            if (NewQuests)
            {
                progresses.Clear();
            }

            CreateQuestTimer(timerType);

            for (int i = 0; i < quantityQuests; i++)
            {
                
                if (progresses.Count < quantityQuests || NewQuests)
                {
                    var data = GenerateRandomQuestData(config, progresses);
                    var progress = new QuestsProgress(0, data.QuestsGuid, false);
                    var model = _questFactory.CreateQuestModel(progress, data);
                    progresses.Add(progress);
                    models.TryAdd(model.Data.QuestsGuid, model);
                }
                else
                {
                    var progress = progresses[i];
                    var data = config.QuestComponents.Find(x => x.QuestsGuid == progress.Guid);
                    var model = _questFactory.CreateQuestModel(progress, data);
                    models.TryAdd(model.Data.QuestsGuid, model);
                }
            }
        }

        public void FillInProgressDailyQuest(QuestsGuid guid, int value)
        {
            if (DailyQuestModels.TryGetValue(guid, out QuestModel model))
            {
                model.FillProgress(value);
                UpdateDailyQuest?.Invoke(model);
            }
        }

        public void FillInProgressWeeklyQuest(QuestsGuid guid, int value)
        {
            if (WeeklyQuestModels.TryGetValue(guid, out QuestModel model))
            {
                model.FillProgress(value);
                UpdateWeeklyQuest?.Invoke(model);
            }
        }

        public void TakeReward(int value)
        {
            _progress.Experience += value;

            if (_progress.Experience >= _staticDataService.BattlePassConfig.ExperienceForLevel)
            {
                var howMuchToUpgrade = _progress.Experience / _staticDataService.BattlePassConfig.ExperienceForLevel;

                for (int i = 0; i < howMuchToUpgrade; i++)
                {
                    _progress.Experience -= _staticDataService.BattlePassConfig.ExperienceForLevel;
                    _progress.Level++;
                }
            }
        }

        private QuestComponent GenerateRandomQuestData(QuestStaticData config, List<QuestsProgress> progresses)
        {
            var randomIndex = Random.Range(0, config.QuestComponents.Count);
            var questData = config.QuestComponents[randomIndex];

            while (progresses.Find(x => x.Guid == questData.QuestsGuid) != null && questData.WIP)
            {
                randomIndex = Random.Range(0, config.QuestComponents.Count);
                questData = config.QuestComponents[randomIndex];
            }
            
            return questData.WIP ? GenerateRandomQuestData(config, progresses) : questData;
        }

        private void CreateQuestTimer(TimerType timerType)
        {
            if (timerType == TimerType.BattlePassDailyTimer)
            {
                var timer = (int)(86400D - DateTime.Now.TimeOfDay.TotalSeconds);
                DailyTimer = _timerService.CreateTimer(timerType.ToString(), timer);
                DailyTimer.Stopped += OnTimerStopped;
            }
            else if (timerType == TimerType.BattlePassWeeklyTimer)
            {
                var dateCreateWeeklyQuests = DateTime.Parse(_progressService.PlayerProgress.DateCreateWeekQuests);
                var timer = (int)(dateCreateWeeklyQuests.AddDays(7) - DateTime.Now).TotalSeconds;

                WeeklyTimer = _timerService.CreateTimer(timerType.ToString(), timer);
                WeeklyTimer.Stopped += OnTimerStopped;
            }
        }

        private void OnTimerStopped(TimeModel timer)
        {
            if (timer.TimeProgress.ID == TimerType.BattlePassWeeklyTimer.ToString())
            {
                _progress.WeeklyQuestProgress.Clear();
                CreateQuests(_weeklyQuestsConfig, _progress.WeeklyQuestProgress, WeeklyQuestModels, true,
                    AMOUNT_WEEKLY_QUESTS, TimerType.BattlePassWeeklyTimer);
                
                RecreatedDailyQuests?.Invoke(false);
            }

            if (timer.TimeProgress.ID == TimerType.BattlePassDailyTimer.ToString())
            {
                _progress.DailyQuestProgress.Clear();
                CreateQuests(_dailyQuestsConfig, _progress.DailyQuestProgress, DailyQuestModels, true,
                    AMOUNT_DAILY_QUESTS, TimerType.BattlePassDailyTimer);
                
                RecreatedDailyQuests?.Invoke(true);
            }
        }
    }
}