using System;
using Gameplay.DailyEntry;
using Infrastructure.PersistenceProgress;

namespace Gameplay.Investing
{
    [Serializable]
    public class InvestingProgress
    {
        public ViewStatusType StatusType;
        public int CountBoost;
        public int Timer;

        public InvestingProgress()
        {
            StatusType = ViewStatusType.Default;
            CountBoost = 0;
        }
    }
}