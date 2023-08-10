using DG.Tweening;
using Infrastructure.Windows;

namespace Gameplay.TutorialStateMachine
{
    public class ShowQuestsState : TutorialState
    {
        private const string FIRST_MESSAGE = "Пока накапливаются ресурсы. Можешь выполнить ежедневные задания.";
        private const string SECOND_MESSAGE = "Выполняя задания ты получаешь <br> ценную награду";

        public ShowQuestsState(TutorialModel tutorialModel, IWindowService windowService)
            : base(tutorialModel, TutorialStageType.ShowQuests, windowService)
        {
        }

        public override void Enter()
        {
            base.Enter();

            TutorialModel.DisplayTutorialMessage(FIRST_MESSAGE);

            DOVirtual.DelayedCall(4, () => { TutorialModel.ShowHighlightHudButton(WindowType.DailyQuest); }, false);

            WindowService.OnOpen += OpenWindow;
            WindowService.OnClosed += ClosedWindow;
            TutorialModel.UpgradeClick += CloseClick;
        }

        public override void Exit()
        {
            base.Exit();

            WindowService.OnOpen -= OpenWindow;
            WindowService.OnClosed -= ClosedWindow;
            TutorialModel.UpgradeClick -= CloseClick;
        }

        private void CloseClick()
        {
            WindowService.Close(WindowType.DailyQuest);
        }


        private void ClosedWindow(WindowType type)
        {
            if (type == WindowType.DailyQuest)
            {
                WindowService.Close(WindowType.Tutorial);
                TutorialModel.SetStage(TutorialStageType.LastMessage);
                StateMachine.Enter<LastMessageTutorialState>();
            }
        }

        private void OpenWindow(WindowType type)
        {
            if (type == WindowType.DailyQuest)
            {
                WindowService.Close(WindowType.Tutorial);
                TutorialModel.DisplayShortTutorialMessage(SECOND_MESSAGE, false, true);

                DOVirtual.DelayedCall(4, () => { TutorialModel.ShowHighlightUpgradeButton(false); });
            }
        }
    }
}