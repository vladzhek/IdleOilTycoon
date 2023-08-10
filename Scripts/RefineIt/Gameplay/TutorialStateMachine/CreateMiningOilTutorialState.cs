using DG.Tweening;
using Gameplay.CameraProject;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Services;
using Gameplay.Workspaces;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.TutorialStateMachine
{
    public class CreateMiningOilTutorialState : TutorialState
    {
        private const string FIRST_MESSAGE = "Для начала построим НЕФТЯНУЮ ВЫШКУ!.";
        private const string SECOND_MESSAGE = "В этом здании добывают и перерабатывают основной ресурс- Нефть.";
        private const string THIRD_MESSAGE = "Отлично!<br>А где мы это будем хранить?!";

        private readonly CameraZoomController _cameraZoomController;
        private readonly ITilemapController _tilemapController;
        private readonly IWorkspaceService _workspaceService;

        private readonly Vector3Int _cameraPosition = new(-5, -2, 0);
        private readonly Vector3Int _miningPosition = new(-6, -3, 0);

        public CreateMiningOilTutorialState(TutorialModel tutorialModel, CameraZoomController cameraZoomController,
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

            TutorialModel.DisplayShortTutorialMessage(FIRST_MESSAGE, true, true);
            _cameraZoomController.ZoomIn(_cameraPosition);

            TutorialModel.ClickFinger += OnClickFinger;
            TutorialModel.UpgradeClick += OnUpgradeBuild;
            _workspaceService.Builded += OnBuilded;
        }

        public override void Exit()
        {
            base.Exit();

            TutorialModel.ClickFinger -= OnClickFinger;
            TutorialModel.UpgradeClick -= OnUpgradeBuild;
            _workspaceService.Builded -= OnBuilded;
        }

        private void OnBuilded(IBuilding building)
        {
            if (building.Id == "Mining")
            {
                TutorialModel.DisplayShortTutorialMessage(THIRD_MESSAGE, false, true);
                
                DOVirtual.DelayedCall(4, () =>
                {
                    WindowService.Close(WindowType.Tutorial);
                    StateMachine.Enter<CreateComplexTutorialState>();
                },false);
            }
        }

        private void OnUpgradeBuild()
        {
            TutorialModel.SetStage(TutorialStageType.CreateComplex);

            WindowService.Close(WindowType.Tutorial);
            _workspaceService.MiningWorkspaces[_miningPosition].Buy();
            WindowService.Open(WindowType.Tutorial);
            _cameraZoomController.ZoomIn(_cameraPosition);
        }

        private void OnClickFinger()
        {
            WindowService.Close(WindowType.Tutorial);

            _tilemapController.OnClick(_miningPosition);

            TutorialModel.DisplayShortTutorialMessage(SECOND_MESSAGE, false, true);
            TutorialModel.ShowHighlightUpgradeButton(true);
        }
    }
}