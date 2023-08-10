using DG.Tweening;
using Infrastructure.Windows;

namespace Gameplay.TutorialStateMachine
{
    public class ShowOrderState : TutorialState
    {
        private const string FIRST_MESSAGE = "Смотри!<br>У тебя появился первый заказ!";
        private const string SECOND_MESSAGE = "Выполняя заказы ты получаешь <br> ценную награду";

        public ShowOrderState(TutorialModel tutorialModel,
            IWindowService windowService) : base(tutorialModel, TutorialStageType.ShowFirstOrder, windowService)
        {
        }

        public override void Enter()
        {
            base.Enter();

            TutorialModel.DisplayTutorialMessage(FIRST_MESSAGE);

            DOVirtual.DelayedCall(4, () =>
            {
                TutorialModel.ShowHighlightHudButton(WindowType.OrdersWindow);
            }, false);

            WindowService.OnOpen += OpenWindow;
            WindowService.OnClosed += ClosedWindow;
            TutorialModel.UpgradeClick += CloseClick;
        }

        public override void Exit()
        {
            base.Exit();

            WindowService.OnOpen -= OpenWindow;
            TutorialModel.UpgradeClick -= CloseClick;
            WindowService.OnClosed -= ClosedWindow;
        }

        private void CloseClick()
        {
            WindowService.Close(WindowType.OrdersWindow);
        }

        private void ClosedWindow(WindowType type)
        {
            if (type == WindowType.OrdersWindow)
            {
                WindowService.Close(WindowType.Tutorial);
                TutorialModel.SetStage(TutorialStageType.ShowQuests);
                StateMachine.Enter<ShowQuestsState>();
            }
        }

        private void OpenWindow(WindowType type)
        {
            if (type == WindowType.OrdersWindow)
            {
                WindowService.Close(WindowType.Tutorial);
                TutorialModel.DisplayShortTutorialMessage(SECOND_MESSAGE, false, true);

                DOVirtual.DelayedCall(4, () =>
                {
                    TutorialModel.ShowHighlightUpgradeButton(false);
                });
            }
        }
    }
}