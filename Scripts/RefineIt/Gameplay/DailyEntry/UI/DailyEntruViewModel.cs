using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Gameplay.Currencies;
using Gameplay.Settings;
using Gameplay.Settings.UI;
using Infrastructure.Windows.MVVM;
using UnityEngine;

namespace Gameplay.DailyEntry.UI
{
    public class DailyEntruViewModel : ViewModelBase<IDailyEntryModel, DailyEntryView>
    {
        public DailyEntruViewModel(IDailyEntryModel model, DailyEntryView view) : base(model, view)
        {
            
        }

        public override Task Show()
        {
            InitializeDailyData();
            return Task.CompletedTask;
        }
        
        public override void Subscribe()
        {
            base.Subscribe();
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            UnSubRewardButtons();
        }

        private void InitializeDailyData()
        {
            var staticData = Model.GetStaticData();
            foreach (var subView in View.subContainer.SubViewList)
            {
                DailyEntryComponentData component = Model.GetComponentData(subView.Day);
                DailyEntryProgress progress = Model.GetProgress(subView.Day);
                var rewardSprite = component.rewardType == CurrencyType.HardCurrency ? staticData.CoinSprite : staticData.DollarSprite;
                
                DailyEntrySubData data = new DailyEntrySubData(rewardSprite, component.description, component.reward,
                    progress.IsTake, progress.IsVisableReward, subView.Day, component.rewardType);
                
                if (component.Day == DailyEntryType.Day7)
                {
                    data.SetSecondReward(staticData.SecondRewardDay7.ToString());
                }
                View.subContainer.UpdateSubData(data,subView.Day);
                View.subContainer.GetSubView(subView.Day).GetRewardButton().Click += Model.TakeReward;
                View.subContainer.GetSubView(subView.Day).GetRewardButton().Click += ShowTakeReward;
            }
        }

        private void UnSubRewardButtons()
        {
            foreach (var subView in View.subContainer.SubViewList)
            {
                View.subContainer.GetSubView(subView.Day).GetRewardButton().Click -= Model.TakeReward;
                View.subContainer.GetSubView(subView.Day).GetRewardButton().Click -= ShowTakeReward;
            }
        }

        private void ShowTakeReward(DailyEntryType day, CurrencyType cur, int amount)
        {
            View.subContainer.GetSubView(day).ActiveCheck(true);
        }
    }
}