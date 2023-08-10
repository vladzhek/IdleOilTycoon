using Gameplay.Currencies;
using Gameplay.Quests;

namespace Gameplay.BattlePass
{
    public class QuestModel
    {
        public QuestsProgress Progress;
        public QuestComponent Data;

        public bool IsCanTakeReward;

        public QuestModel(QuestComponent data, QuestsProgress progress)
        {
            Data = data;
            Progress = progress;

            IsCanTakeReward = Progress.QuestProgress >= Data.Count && !Progress.IsTakeReward;
        }

        public void FillProgress(int value)
        {
            Progress.QuestProgress += value;
            
            if (Progress.QuestProgress >= Data.Count)
            {
                Progress.QuestProgress = Data.Count;
                
                IsCanTakeReward = true;
            }
        }

        public void CompleteQuest()
        {
            Progress.IsTakeReward = true;
        }
    }
}