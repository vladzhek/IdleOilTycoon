using System;

namespace Gameplay.TutorialStateMachine
{
    [Serializable]
    public class TutorialProgress
    {
        public TutorialStageType StageType = TutorialStageType.Completed;
    }
}