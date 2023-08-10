using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;
using Infrastructure.Windows;

namespace Gameplay.TutorialStateMachine
{
    public abstract class TutorialState : IState
    {
        protected readonly TutorialModel TutorialModel;
        protected readonly IWindowService WindowService;
        protected IStateMachine StateMachine;
        protected TutorialStageType StageType;

        protected TutorialState(TutorialModel tutorialModel, TutorialStageType stageType, IWindowService windowService)
        {
            TutorialModel = tutorialModel;
            StageType = stageType;
            WindowService = windowService;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Exit(){}

        public virtual void Enter()
        {
        }
    }
}