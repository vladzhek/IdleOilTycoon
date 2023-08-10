using Infrastructure.Windows.MVVM;

namespace Gameplay.RewardPopUp
{
    public class RewardPopupViewModelFactory : IViewModelFactory<RewardPopupViewModel, RewardPopupView, RewardPopupModel>
    {
        public RewardPopupViewModel Create(RewardPopupModel model, RewardPopupView view)
        {
            return new RewardPopupViewModel(model, view);
        }
    }
}