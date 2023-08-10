using System.Threading.Tasks;
using Gameplay.Tilemaps.Data;
using Infrastructure.Windows;
using Infrastructure.Windows.MVVM;

namespace Gameplay.TutorialStateMachine
{
    public class TutorialViewModel : ViewModelBase<TutorialModel, TutorialView>
    {
        public TutorialViewModel(TutorialModel model, TutorialView view) : base(model, view)
        {
        }

        public override Task Show()
        {
            return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            
            Model.Message += OnMessage;
            Model.ShortMessage += OnShortMessage;
            Model.OnShowCloseTarget += ShowCloseTarget;
            Model.HighlightHudButton += OnHighlightHudButton;
            Model.HighlightUpgradeButton += OnHighlightUpgradeButton;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            
            Model.Message -= OnMessage;
            Model.ShortMessage -= OnShortMessage;
            Model.OnShowCloseTarget -= ShowCloseTarget;
            Model.HighlightHudButton -= OnHighlightHudButton;
            Model.HighlightUpgradeButton -= OnHighlightUpgradeButton;
        }

        private void OnHighlightUpgradeButton(bool isUpgradeButton)
        {
            View.DisableTutorialGameObjects();
            View.HighlightBackButton(!isUpgradeButton);
            View.HighlightUpgradeButton(isUpgradeButton);
        }

        private void OnHighlightHudButton(WindowType type)
        {
            View.DisableTutorialGameObjects();
            View.HighlightHudButton(type);
        }

        private void ShowCloseTarget(bool isActive, BuildingType type)
        {
            View.DisableTutorialGameObjects();
            View.ShowCloseMiningTarget(isActive, type);
        }

        private void OnMessage(string message)
        {
            View.DisableTutorialGameObjects();
            View.SetMessage(message);
        }
        
        private void OnShortMessage(string message, bool isFinger, bool isNeedBackground)
        {
            View.DisableTutorialGameObjects();
            View.SetShortMessage(message, isFinger, isNeedBackground);
        }
    }
}