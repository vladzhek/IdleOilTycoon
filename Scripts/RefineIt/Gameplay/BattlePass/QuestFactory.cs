using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Quests;

namespace Gameplay.BattlePass
{
    public class QuestFactory : IQuestFactory
    {
        public QuestModel CreateQuestModel(QuestsProgress progress, QuestComponent data)
        {
            return new QuestModel(data, progress);
        }
    }
}