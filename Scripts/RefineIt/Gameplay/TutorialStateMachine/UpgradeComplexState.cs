using DG.Tweening;
using Gameplay.CameraProject;
using Gameplay.Tilemaps.Data;
using Gameplay.Tilemaps.Services;
using Gameplay.Workspaces;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.TutorialStateMachine
{
    public class UpgradeComplexState : TutorialState
    {
        private const string Message = "Отлично!<br> Давай  теперь его улучшим";
        private const string SECOND_MESSAGE = "Улучшая увеличивается<br>объем комплекса.";

        private readonly CameraZoomController _cameraZoomController;
        private readonly ITilemapController _tilemapController;
        private readonly IWorkspaceService _workspaceService;

        private readonly Vector3Int _cameraPosition = new(-5, -9, 0);
        private readonly Vector3Int _complexPosition = new(-5, -10, 0);

        public UpgradeComplexState(TutorialModel tutorialModel, CameraZoomController cameraZoomController,
            ITilemapController tilemapController, IWindowService windowService, IWorkspaceService workspaceService) :
            base(tutorialModel, TutorialStageType.ShowMiningOil, windowService)
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
            TutorialModel.UpgradeClick += OnUpgradeBuild;
            WindowService.OnClosed += OnClosed;
        }

        public override void Exit()
        {
            base.Exit();

            TutorialModel.ClickFinger -= ClickOnBuild;
            TutorialModel.UpgradeClick -= OnUpgradeBuild;
            WindowService.OnClosed -= OnClosed;
        }

        private void ClickOnBuild()
        {
            WindowService.Close(WindowType.Tutorial);
            _tilemapController.OnClick(_complexPosition);
            TutorialModel.DisplayShortTutorialMessage(SECOND_MESSAGE, false, true);

            TutorialModel.ShowHighlightUpgradeButton(true);
        }

        private void OnClosed(WindowType type)
        {
            if (type == WindowType.ComplexWindow)
            {
                WindowService.Close(WindowType.Tutorial);
                TutorialModel.SetStage(TutorialStageType.CreatedProcessing);

                StateMachine.Enter<CreateProcessingState>();
            }
        }

        private void OnUpgradeBuild()
        {
            _workspaceService.ComplexWorkspaceModels[_complexPosition].UpdateLevel();
            TutorialModel.ShowCloseTarget(true, BuildingType.Complex);
        }
    }
}