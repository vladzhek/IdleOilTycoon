using Infrastructure.Windows.MVVM;

namespace Gameplay.TutorialStateMachine
{
    public class TutorialViewModelFactory : IViewModelFactory<TutorialViewModel, TutorialView, TutorialModel>
    {
        public TutorialViewModel Create(TutorialModel model, TutorialView view)
        {
            return new TutorialViewModel(model, view);
        }
    }
}