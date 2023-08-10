using Gameplay.CameraProject;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Services;
using Gameplay.Workspaces;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.TutorialStateMachine
{
    public class CreateProcessingState : TutorialState
    {
        private const string FIRST_MESSAGE = "Теперь нам нужен <br> ЦЕХ ПЕРЕРАБОТКИ.";
        private const string SECOND_MESSAGE = "Тут нефть, как базовое сырье превращается в другие полезные ресурсы!";

        private readonly CameraZoomController _cameraZoomController;
        private readonly ITilemapController _tilemapController;
        private readonly IWorkspaceService _workspaceService;

        private readonly Vector3Int _cameraPosition = new(-1, -10, 0);
        private readonly Vector3Int _buildingPosition = new(-2, -11, 0);

        public CreateProcessingState(TutorialModel tutorialModel, CameraZoomController cameraZoomController,
            ITilemapController tilemapController, IWindowService windowService, IWorkspaceService workspaceService) :
            base(tutorialModel, TutorialStageType.ShowProcessing, windowService)
        {
            _cameraZoomController = cameraZoomController;
            _tilemapController = tilemapController;
            _workspaceService = workspaceService;
        }

        public override void Enter()
        {
            base.Enter();

            _cameraZoomController.ZoomIn(_cameraPosition);

            TutorialModel.DisplayShortTutorialMessage(FIRST_MESSAGE, true, true);

            TutorialModel.ClickFinger += ClickOnBuild;
            TutorialModel.UpgradeClick += OnUpgradeBuild;
            _workspaceService.Builded += OnBuilded;
        }

        public override void Exit()
        {
            base.Exit();

            TutorialModel.ClickFinger -= ClickOnBuild;
            TutorialModel.UpgradeClick -= OnUpgradeBuild;
            _workspaceService.Builded -= OnBuilded;
        }

        private void ClickOnBuild()
        {
            WindowService.Close(WindowType.Tutorial);
            _tilemapController.OnClick(_buildingPosition);
            TutorialModel.DisplayShortTutorialMessage(SECOND_MESSAGE, false, true);
            TutorialModel.ShowHighlightUpgradeButton(true);
        }

        private void OnBuilded(IBuilding building)
        {
            if (building.Id == "Nafta")
            {
                WindowService.Close(WindowType.Tutorial);
                StateMachine.Enter<UpgradeProcessingState>();
            }
        }

        private void OnUpgradeBuild()
        {
            WindowService.Close(WindowType.Tutorial);
            _workspaceService.ProcessingWorkspaces[_buildingPosition].Buy();
            TutorialModel.SetStage(TutorialStageType.UpgradeComplex);
            WindowService.Open(WindowType.Tutorial);
            _cameraZoomController.ZoomIn(_cameraPosition);
        }
    }
}