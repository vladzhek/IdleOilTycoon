using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using MVVMLibrary.Base.ViewModel;

namespace Gameplay.BattlePass
{
    [Serializable]
    public class BattlePassProgressData
    {
        public bool IsBuy;
        public int Level ;
        public int Experience;

        public List<QuestsProgress> DailyQuestProgress = new();
        public List<QuestsProgress> WeeklyQuestProgress = new();
        
        public List<BattlePassRewardProgress> FreeRewards = new();
        public List<BattlePassRewardProgress> PremiumRewards = new();
    }
}