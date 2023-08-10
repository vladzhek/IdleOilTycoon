using Gameplay.Currencies;
using Gameplay.Quests;
using UnityEngine;

namespace Gameplay.Region.Quests
{
    public class QuestSubData
    {
        public Sprite Sprite;
        public string Description;
        public int Reward;
        public int Count;
        public int Progress;
        public QuestsGuid Guid;
        public bool IsDaily;
        public Sprite RewardSprite;
        public bool IsTakeReward;
        
        public QuestSubData(Sprite sprite, string description, QuestsGuid guid, int reward, int count, int progress
        , bool isDaily)
        {
            Sprite = sprite;
            Description = description;
            Guid = guid;
            Reward = reward;
            Count = count;
            Progress = progress;
            IsDaily = isDaily;
        }

        public void SetRewardSprite(Sprite rewardSprite)
        {
            RewardSprite = rewardSprite;
        }
    }
}