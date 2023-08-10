using DG.Tweening;
using Gameplay.CameraProject;
using Gameplay.Tilemaps.Data;
using Gameplay.Tilemaps.Services;
using Gameplay.Workspaces;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.TutorialStateMachine
{
    public class UpgradeProcessingState : TutorialState
    {
        private const string Message = "Супер!<br> Не забывай улучшать здания";

        private const string SECOND_MESSAGE =
            "Каждая площадка уже<br>подготовлена под добычу<br>определенного ресурса.";

        private readonly CameraZoomController _cameraZoomController;
        private readonly ITilemapController _tilemapController;
        private readonly IWorkspaceService _workspaceService;

        private readonly Vector3Int _cameraPosition = new(-1, -10, 0);
        private readonly Vector3Int _buildingPosition = new(-2, -11, 0);

        public UpgradeProcessingState(TutorialModel tutorialModel, CameraZoomController cameraZoomController,
            ITilemapController tilemapController, IWindowService windowService, IWorkspaceService workspaceService)
            : base(tutorialModel, TutorialStageType.ShowMiningOil, windowService)
        {
            _cameraZoomController = cameraZoomController;
            _tilemapController = tilemapController;
            _workspaceService = workspaceService;
        }

        public override void Enter()
        {
            base.Enter();

            TutorialModel.DisplayShortTutorialMessage(Message, true, true);
            _cameraZoomController.ZoomIn(_cameraPosition);

            TutorialModel.ClickFinger += ClickOnBuild;
            WindowService.OnClosed += OnClosed;
            TutorialModel.UpgradeClick += OnUpgradeBuild;
        }

        public override void Exit()
        {
            base.Exit();

            TutorialModel.ClickFinger -= ClickOnBuild;
            WindowService.OnClosed -= OnClosed;
            TutorialModel.UpgradeClick -= OnUpgradeBuild;
        }

        private void ClickOnBuild()
        {
            WindowService.Close(WindowType.Tutorial);
            _tilemapController.OnClick(_buildingPosition);
            //TutorialModel.DisplayShortTutorialMessage(SECOND_MESSAGE, false,false);

            TutorialModel.ShowHighlightUpgradeButton(true);
        }

        private void OnClosed(WindowType type)
        {
            if (type == WindowType.ProcessingWindow)
            {
                WindowService.Close(WindowType.Tutorial);
                StateMachine.Enter<ShowOrderState>();
            }
        }

        private void OnUpgradeBuild()
        {
            _workspaceService.ProcessingWorkspaces[_buildingPosition].UpdateLevel();
            WindowService.Open(WindowType.Tutorial);
            TutorialModel.ShowCloseTarget(true, BuildingType.Process);
            TutorialModel.SetStage(TutorialStageType.ShowFirstOrder);
        }
    }
}