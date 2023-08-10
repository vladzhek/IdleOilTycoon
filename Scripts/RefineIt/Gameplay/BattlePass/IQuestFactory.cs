using Gameplay.Currencies;
using Gameplay.Quests;

namespace Gameplay.BattlePass
{
    public interface IQuestFactory
    {
        QuestModel CreateQuestModel(QuestsProgress progress, QuestComponent data);
    }
}