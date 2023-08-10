using System;
using System.Collections.Generic;
using Infrastructure.PersistenceProgress;
using UnityEngine;

namespace Gameplay.Quests
{
    public interface IQuestModel
    {
        event Action<QuestsGuid, bool> IsRefreshQuest;
        event Action UpdateProgressDaily;
        
        void StartSession();
        void TaskDailyProgress(QuestsGuid guid,int count);
        Dictionary<QuestsGuid, QuestComponent> GetDailyQuests();
        Dictionary<QuestsGuid, GameObject> GetRefreshButtons(bool isDaily);
        QuestComponent RefreshDailyData(QuestsGuid guid);
        void TakeDailyReward(int reward,QuestsGuid guid);
        void RefreshQuests(QuestsGuid guid, bool isDaily);
        int GetCountRefreshQuests(bool isDaily);
        void AddRefreshButtons(QuestsGuid guid,GameObject button, bool isDaily);
        
        event Action UpdateProgressWeekly;
        void TaskWeeklyProgress(QuestsGuid guid, int count);
        void TakeWeeklyReward(int reward, QuestsGuid guid);
        Dictionary<QuestsGuid, QuestComponent> GetWeeklyQuests();
        event Action ClearViewModel;
        bool isAvailableRewards();
        event Action TakeReward;
        event Action<int> TimerTick;
        PlayerProgress GetProgressService();
    }
}