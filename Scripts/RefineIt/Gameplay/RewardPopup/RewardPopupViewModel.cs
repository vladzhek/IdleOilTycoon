using System.Threading.Tasks;
using Infrastructure.Windows.MVVM;

namespace Gameplay.RewardPopUp
{
    public class RewardPopupViewModel : ViewModelBase<RewardPopupModel, RewardPopupView>
    {
        public RewardPopupViewModel(RewardPopupModel model, RewardPopupView view) : base(model, view)
        {
        }

        public override Task Show()
        {
            View.RewardPopupSubViewContainer.CleanUp();
            for (int index = 0; index < Model.RewardData.Rewards.Count; index++)
            {
                RewardData data = Model.RewardData.Rewards[index];
                View.RewardPopupSubViewContainer.Add(index.ToString(), data);
            }
            
            View.SetLootBoxSprite(Model.RewardData.LootBoxSprite);

            return Task.CompletedTask;
        }
    }
}