using DG.Tweening;
using Infrastructure.Windows;

namespace Gameplay.TutorialStateMachine
{
    public class LastMessageTutorialState : TutorialState
    {
        private const string MESSAGE = "Отлично! Теперь ты готов управлять<br>целым городом! Удачи!";
        
        public LastMessageTutorialState(TutorialModel tutorialModel, IWindowService windowService)
            : base(tutorialModel, TutorialStageType.LastMessage, windowService)
        { }
        
        public override void Enter()
        {
            base.Enter();
            
            TutorialModel.DisplayTutorialMessage(MESSAGE);

            DOVirtual.DelayedCall(4, () =>
            {
                WindowService.Close(WindowType.Tutorial);
                TutorialModel.SetStage(TutorialStageType.Completed);
            }, false);
        }
    }
}