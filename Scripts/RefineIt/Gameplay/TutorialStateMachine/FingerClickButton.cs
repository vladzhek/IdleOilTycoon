using Gameplay.Region;
using Zenject;

namespace Gameplay.TutorialStateMachine
{
    public class FingerClickButton : ButtonBase
    {
        private TutorialModel _tutorialModel;

        [Inject]
        public void Construct(TutorialModel tutorialModel)
        {
            _tutorialModel = tutorialModel;
        }
        
        public override void OnClick()
        {
            _tutorialModel.OnClickTutorialFinger();
        }
    }
}