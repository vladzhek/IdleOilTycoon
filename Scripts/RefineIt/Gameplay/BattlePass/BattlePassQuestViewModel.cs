using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Quests;
using Gameplay.Region.Quests;
using Infrastructure.Windows.MVVM;
using Utils.Extensions;

namespace Gameplay.BattlePass
{
    public class BattlePassQuestViewModel : ViewModelBase<BattlePassQuestModel, BattlePassQuestView>
    {
        private const int REWARD_FOR_DAILY = 25;
        private const int REWARD_FOR_WEEKLY = 50;

        private readonly BattlePassModel _battlePassModel;

        public BattlePassQuestViewModel(BattlePassQuestModel model, BattlePassQuestView view,
            BattlePassModel battlePassModel) : base(model, view)
        {
            _battlePassModel = battlePassModel;
        }

        public override Task Show()
        {
            UpdateExperience();
            return Task.CompletedTask;
        }

        private void UpdateExperience()
        {
            View.SetLevel(Model.Progress.Level);
            View.SetExperienceSlider(Model.BattlePassConfig.ExperienceForLevel, Model.Progress.Experience);
        }

        public override void Subscribe()
        {
            base.Subscribe();

            InitializeSubViews(Model.DailyQuestModels, View.DailyQuestContainer, REWARD_FOR_DAILY);
            InitializeSubViews(Model.WeeklyQuestModels, View.WeeklyQuestContainer, REWARD_FOR_WEEKLY);

            Model.UpdateDailyQuest += OnUpdateDailyQuest;
            Model.UpdateWeeklyQuest += OnUpdateWeeklyQuest;
            Model.RecreatedDailyQuests += OnRecreatedDailyQuests;
            Model.DailyTimer.Tick += OnDailyTimerTick;
            Model.WeeklyTimer.Tick += OnWeeklyTimerTick;

            _battlePassModel.Timer.Tick += OnBattlePassTimerTick;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            Model.UpdateDailyQuest -= OnUpdateDailyQuest;
            Model.UpdateWeeklyQuest -= OnUpdateWeeklyQuest;
            Model.RecreatedDailyQuests -= OnRecreatedDailyQuests;
            Model.DailyTimer.Tick -= OnDailyTimerTick;
            Model.WeeklyTimer.Tick -= OnWeeklyTimerTick;

            _battlePassModel.Timer.Tick -= OnBattlePassTimerTick;
        }

        private void OnUpdateDailyQuest(QuestModel model)
        {
            UpdateQuest(View.DailyQuestContainer, model, REWARD_FOR_DAILY);
        }

        private void OnUpdateWeeklyQuest(QuestModel model)
        {
            UpdateQuest(View.WeeklyQuestContainer, model, REWARD_FOR_WEEKLY);
        }

        private void UpdateQuest(BattlePassQuestContainer dailyQuestContainer, QuestModel model, int rewardValue)
        {
            var subView = dailyQuestContainer.SubViews[model.Data.QuestsGuid.ToString()];
            subView.ActiveRewardButton(model.IsCanTakeReward);
            var description = CreateDescription(model.Data.description, model.Data.Count);
            var data = new QuestSubData(model.Data.Sprite, description, model.Data.QuestsGuid,
                rewardValue, model.Data.Count, model.Progress.QuestProgress, true);
            data.IsTakeReward = model.Progress.IsTakeReward;

            dailyQuestContainer.UpdateView(data, model.Data.QuestsGuid.ToString());
        }

        private void InitializeSubViews(Dictionary<QuestsGuid, QuestModel> quests, BattlePassQuestContainer container,
            int rewardValue)
        {
            container.CleanUp();
            foreach (var quest in quests)
            {
                CreateQuestSubView(quest, container, rewardValue);
            }
        }

        private void CreateQuestSubView(KeyValuePair<QuestsGuid, QuestModel> quest, BattlePassQuestContainer container,
            int rewardValue)
        {
            var description = CreateDescription(quest.Value.Data.description, quest.Value.Data.Count);
            var data = new QuestSubData(quest.Value.Data.Sprite, description, quest.Key, rewardValue,
                quest.Value.Data.Count, quest.Value.Progress.QuestProgress,
                true);
            data.IsTakeReward = quest.Value.Progress.IsTakeReward;

            container.Add(quest.Key.ToString(), data);
            var subView = container.SubViews[quest.Key.ToString()];

            subView.ActiveRewardButton(quest.Value.IsCanTakeReward);

            subView.ButtonTake.ClickButton += TakeReward;
        }
        
        private void OnRecreatedDailyQuests(bool isDailyQuest)
        {
            if (isDailyQuest)
            {
                InitializeSubViews(Model.DailyQuestModels, View.DailyQuestContainer, REWARD_FOR_DAILY);
            }
            else
            {
                InitializeSubViews(Model.WeeklyQuestModels, View.WeeklyQuestContainer, REWARD_FOR_WEEKLY);
            }
        }

        private void TakeReward(int value, QuestsGuid guid)
        {
            if (Model.DailyQuestModels.TryGetValue(guid, out QuestModel questModel))
            {
                Model.TakeReward(value);
                questModel.CompleteQuest();
                View.DailyQuestContainer.SubViews[guid.ToString()].HideRewardButton(questModel.Progress.IsTakeReward);
            }

            if (Model.WeeklyQuestModels.TryGetValue(guid, out QuestModel model))
            {
                Model.TakeReward(value);
                model.CompleteQuest();
                View.WeeklyQuestContainer.SubViews[guid.ToString()].HideRewardButton(model.Progress.IsTakeReward);
            }

            UpdateExperience();
        }

        private string CreateDescription(string description, int count)
        {
            return description.Replace("[N]", count.ToString());
        }

        private void OnBattlePassTimerTick(int time)
        {
            View.SetBattlePassTimer(FormatTime.DayAndHoursToString(time));
        }

        private void OnDailyTimerTick(int time)
        {
            View.SetDailyTimer(FormatTime.HoursStringFormat(time));
        }

        private void OnWeeklyTimerTick(int time)
        {
            View.SetWeeklyTimer(FormatTime.DayAndHoursToString(time));
        }
    }
}