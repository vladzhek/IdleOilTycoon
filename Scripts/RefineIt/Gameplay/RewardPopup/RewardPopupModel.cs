using Infrastructure.Windows;

namespace Gameplay.RewardPopUp
{
    public class RewardPopupModel
    {
        private readonly IWindowService _windowService;

        public RewardPopupModel(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public RewardsPopupData RewardData { get; private set; } = new();

        public async void ShowRewardPopUp(RewardsPopupData data)
        {
            RewardData = data;
            await _windowService.Open(WindowType.RewardPopup);
        }
    }
}