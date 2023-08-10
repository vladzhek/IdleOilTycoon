using System;
using Gameplay.Quests;

namespace Gameplay.Investing.UI
{
    public interface IInvestingModel
    {
        event Action<int> OnTimerTick;
        event Action OnTimerStopped;
        void StartInvestingProcess();
        void BoostInvestingProcess();
        void GetInvestingReward();
        int GetAmountForReward();
        InvestingProgress GetProgressData();
        InvestingStaticData GetStaticData();
        void Initialize();
    }
}