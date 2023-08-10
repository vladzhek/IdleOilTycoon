using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Gameplay.Region.Quests;
using Infrastructure.PersistenceProgress;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;
using Utils.Extensions;

namespace Gameplay.Quests.UI
{
    public class WeeklyQuestViewModel : ViewModelBase<IQuestModel, WeeklyQuestView>
    {
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticDataService;
        private PlayerProgress _progress;
        public WeeklyQuestViewModel(IAssetProvider assetProvider, IStaticDataService staticDataService, IQuestModel model, WeeklyQuestView view) : base(model, view)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _progress = Model.GetProgressService();
        }

        public override Task Show()
        {
            ShowWeeklySubView();
            HideRefreshButtons(QuestsGuid.allDaily,false);
            return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            Model.IsRefreshQuest += RefreshWeeklySubView;
            Model.IsRefreshQuest += HideRefreshButtons;
            Model.UpdateProgressWeekly += UpdateSubViews;
            Model.TimerTick += UpdateTimer;
        }

        public override void Unsubscribe()
        {
            Model.IsRefreshQuest -= RefreshWeeklySubView;
            Model.IsRefreshQuest -= HideRefreshButtons;
            Model.UpdateProgressWeekly -= UpdateSubViews;
            Model.TimerTick -= UpdateTimer;
            UnsubscribeButton();
        }

        private void UpdateTimer(int tick)
        {
            var dateCreateWeek = DateTime.Parse(_progress.DateCreateWeekQuests);
            var date = DateTime.Today - dateCreateWeek.Date;
            View.TimerText.text = FormatTime.DayAndHoursToString(tick+(date.Days * 86400));
        }

        private void ShowWeeklySubView()
        {
            var quests = Model.GetWeeklyQuests();
                                
            foreach (var quest in quests)
            {
                var description = CreateDescription(quest.Value.description, quest.Value.Count);
                var data = new QuestSubData(quest.Value.Sprite, description, quest.Key
                    , quest.Value.Reward, quest.Value.Count, quest.Value.QuestProgress.QuestProgress, false);

                data.SetRewardSprite(_assetProvider.LoadSprite(_staticDataService.GetCurrencyData(quest.Value.rewardType).Sprite).Result);
                View.WeeklyQuestContainer.Add(quest.Key.ToString(), data);

                if (quest.Value.QuestProgress.QuestProgress >= quest.Value.Count)
                {
                    View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].ActiveRewardButton(true);
                    View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].HideRefreshButton();
                }

                if (quest.Value.QuestProgress.IsTakeReward)
                {
                    View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].HideRewardButton();
                }

                View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].ButtonTake.ClickButton += Model.TakeWeeklyReward;
                View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].ButtonAds.ClickButton += Model.RefreshQuests;
                
                var refreshBtn = View.WeeklyQuestContainer.SubViews[quest.Key.ToString()].GetRefreshGameObject();
                Model.AddRefreshButtons(quest.Key, refreshBtn, false);
            }
        }

        private void UnsubscribeButton()
        {
            foreach (var daily in View.WeeklyQuestContainer.SubViews.Values)
            {
                daily.ButtonTake.ClickButton -= Model.TakeWeeklyReward;
                daily.ButtonAds.ClickButton -= Model.RefreshQuests;
            }
        }

        private void UpdateSubViews()
        {
            var quests = Model.GetWeeklyQuests();
            foreach (var subView in View.WeeklyQuestContainer.SubViews)
            {
                var subGuid = (QuestsGuid)Enum.Parse(typeof(QuestsGuid), subView.Key);   
                var quest = quests[subGuid];
                var description = CreateDescription(quest.description, quest.Count);

                var data = new QuestSubData(quest.Sprite, description, quest.QuestsGuid
                    , quest.Reward, quest.Count, quest.QuestProgress.QuestProgress, false);

                data.SetRewardSprite(_assetProvider.LoadSprite(_staticDataService.GetCurrencyData(quest.rewardType).Sprite).Result);
                View.WeeklyQuestContainer.UpdateView(data, subView.Key);
                if (quest.QuestProgress.QuestProgress >= quest.Count)
                {
                    View.WeeklyQuestContainer.SubViews[subView.Key].ActiveRewardButton(true);
                    View.WeeklyQuestContainer.SubViews[subView.Key].HideRefreshButton();
                }
            }
        }

        private void RefreshWeeklySubView(QuestsGuid guid, bool isDaily)
        {
            if(isDaily) return;
            
            var quest = Model.RefreshDailyData(guid);
            
            if(quest == null) return;
            
            var description = CreateDescription(quest.description, quest.Count);
            var data = new QuestSubData(quest.Sprite, description, quest.QuestsGuid
                , quest.Reward, quest.Count, quest.QuestProgress.QuestProgress, false);

            data.SetRewardSprite(_assetProvider.LoadSprite(_staticDataService.GetCurrencyData(quest.rewardType).Sprite).Result);
            Model.GetRefreshButtons(false).Remove(guid);
            View.WeeklyQuestContainer.Remove(guid.ToString());
            
            View.WeeklyQuestContainer.Add(quest.QuestsGuid.ToString(), data);
            View.WeeklyQuestContainer.SubViews[quest.QuestsGuid.ToString()].ButtonAds.ClickButton += Model.RefreshQuests;
            var refreshBtn = View.WeeklyQuestContainer.SubViews[quest.QuestsGuid.ToString()].GetRefreshGameObject();
            Model.AddRefreshButtons(quest.QuestsGuid, refreshBtn, false);
        }

        private void HideRefreshButtons(QuestsGuid guid, bool isDaily)
        {
            var buttons = Model.GetRefreshButtons(false);
            buttons[QuestsGuid.allWeekly].gameObject.SetActive(false);
            buttons[QuestsGuid.doneDaily].gameObject.SetActive(false);
            
            if (Model.GetCountRefreshQuests(false) <= 0)
            {
                
                foreach (var button in buttons)
                {
                    button.Value.SetActive(false);
                }
            }
        }

        private string CreateDescription(string description, int count)
        {
            return description.Replace("[N]", count.ToString());
        }
    }
}