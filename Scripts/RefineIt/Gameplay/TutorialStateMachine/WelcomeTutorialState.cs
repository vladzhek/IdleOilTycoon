using DG.Tweening;
using Infrastructure.Windows;

namespace Gameplay.TutorialStateMachine
{
    public class WelcomeTutorialState : TutorialState
    {
        private const string Message =
            "Приветствую! Меня зовут Сэм.<br>Добро пожаловать в наш индустриальный город!<br>Я покажу что к чему.";

        public WelcomeTutorialState(TutorialModel tutorialModel, IWindowService windowService) 
            : base(tutorialModel, TutorialStageType.Welcome, windowService)
        {
        }

        public override void Exit()
        {
        }

        public override void Enter()
        {
            base.Enter();

            TutorialModel.DisplayTutorialMessage(Message);

            DOVirtual.DelayedCall(5, () =>
            {
                TutorialModel.SetStage(TutorialStageType.ShowMiningOil);
                StateMachine.Enter<CreateMiningOilTutorialState>();
            },false);
        }
    }
}