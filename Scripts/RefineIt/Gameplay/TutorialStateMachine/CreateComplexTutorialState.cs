using Gameplay.CameraProject;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Services;
using Gameplay.Workspaces;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.TutorialStateMachine
{
    public class CreateComplexTutorialState : TutorialState
    {
        private const string FIRST_MESSAGE = "Нам нужно построить<br>КОМПЛЕКС ТАНКЕРОВ";

        private const string SECOND_MESSAGE = "В комплексе хранятся ресурсы ресурсы.";

        private readonly CameraZoomController _cameraZoomController;
        private readonly ITilemapController _tilemapController;
        private readonly IWorkspaceService _workspaceService;

        private readonly Vector3Int _cameraPosition = new(-5, -9, 0);
        private readonly Vector3Int _complexPosition = new(-5, -10, 0);

        public CreateComplexTutorialState(TutorialModel tutorialModel, CameraZoomController cameraZoomController,
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
            _workspaceService.Builded -= OnBuilded;
            TutorialModel.UpgradeClick -= OnUpgradeBuild;
        }

        private void ClickOnBuild()
        {
            WindowService.Close(WindowType.Tutorial);
            _tilemapController.OnClick(_complexPosition);

            TutorialModel.DisplayShortTutorialMessage(SECOND_MESSAGE, false, true);
            TutorialModel.ShowHighlightUpgradeButton(true);
        }

        private void OnUpgradeBuild()
        {
            TutorialModel.SetStage(TutorialStageType.UpgradeComplex);

            WindowService.Close(WindowType.Tutorial);
            _workspaceService.ComplexWorkspaceModels[_complexPosition].Buy();
            WindowService.Open(WindowType.Tutorial);
            _cameraZoomController.ZoomIn(_cameraPosition);
        }

        private void OnBuilded(IBuilding building)
        {
            if (building.Id == "Tanker")
            {
                WindowService.Close(WindowType.Tutorial);
                StateMachine.Enter<UpgradeComplexState>();
            }
        }
    }
}