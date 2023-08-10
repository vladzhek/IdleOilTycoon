using System;
using Gameplay.CameraProject;
using Gameplay.Tilemaps.Data;
using Gameplay.Tilemaps.Services;
using Gameplay.Workspaces;
using Infrastructure.PersistenceProgress;
using Infrastructure.StateMachine;
using Infrastructure.Windows;

namespace Gameplay.TutorialStateMachine
{
    public class TutorialModel
    {
        public event Action<string> Message;
        public event Action<string, bool, bool> ShortMessage;
        public event Action<bool, BuildingType> OnShowCloseTarget;
        public event Action<WindowType> HighlightHudButton;
        public event Action<bool> HighlightUpgradeButton;
        public event Action ClickFinger;
        public event Action UpgradeClick;

        private readonly IWindowService _windowService;
        private readonly IProgressService _progressService;
        private readonly CameraZoomController _cameraZoomController;
        private readonly ITilemapController _tilemapController;
        private readonly IWorkspaceService _workspaceService;

        private StateMachine _stateMachine = new();

        public TutorialModel(IWindowService windowService, IProgressService progressService,
            CameraZoomController cameraZoomController, ITilemapController tilemapController,
            IWorkspaceService workspaceService)
        {
            _windowService = windowService;
            _progressService = progressService;
            _cameraZoomController = cameraZoomController;
            _tilemapController = tilemapController;
            _workspaceService = workspaceService;
        }

        public TutorialStageType StageType => _progressService.PlayerProgress.TutorialProgress.StageType;

        public void Initialize()
        {
            if (_progressService.IsLoaded)
            {
                OnLoaded();
            }
            else
            {
                _progressService.OnLoaded += OnLoaded;
            }
        }

        private void OnLoaded()
        {
            var welcomeState = new WelcomeTutorialState(this, _windowService);
            var showMiningState =
                new CreateMiningOilTutorialState(this, _cameraZoomController, _tilemapController, _windowService,
                    _workspaceService);
            var showComplex = new CreateComplexTutorialState(this, _cameraZoomController, _tilemapController,
                _windowService, _workspaceService);
            var upgradeComplex = new UpgradeComplexState(this, _cameraZoomController, _tilemapController,
                _windowService, _workspaceService);
            var showProcessing = new CreateProcessingState(this, _cameraZoomController, _tilemapController,
                _windowService, _workspaceService);
            var upgradeProcessingState = new UpgradeProcessingState(this, _cameraZoomController, _tilemapController,
                _windowService, _workspaceService);
            var showFirstOrder = new ShowOrderState(this, _windowService);
            var showQuest = new ShowQuestsState(this, _windowService);
            var lastMessage = new LastMessageTutorialState(this, _windowService);

            _stateMachine.AddState(welcomeState);
            _stateMachine.AddState(showMiningState);
            _stateMachine.AddState(showComplex);
            _stateMachine.AddState(showProcessing);
            _stateMachine.AddState(showFirstOrder);
            _stateMachine.AddState(showQuest);
            _stateMachine.AddState(upgradeComplex);
            _stateMachine.AddState(upgradeProcessingState);
            _stateMachine.AddState(lastMessage);

            EnterState();
        }

        public void SetStage(TutorialStageType stageType)
        {
            _progressService.PlayerProgress.TutorialProgress.StageType = stageType;
        }

        public async void DisplayTutorialMessage(string message)
        {
            if (!_windowService.IsOpen(WindowType.Tutorial))
            {
                await _windowService.Open(WindowType.Tutorial);
            }

            Message?.Invoke(message);
        }

        public async void DisplayShortTutorialMessage(string message, bool isFinger, bool isBlock)
        {
            if (!_windowService.IsOpen(WindowType.Tutorial))
            {
                await _windowService.Open(WindowType.Tutorial);
            }

            ShortMessage?.Invoke(message, isFinger, isBlock);
        }

        public void ShowCloseTarget(bool IsActive, BuildingType buildingType)
        {
            OnShowCloseTarget?.Invoke(IsActive, buildingType);
        }

        public async void ShowHighlightUpgradeButton(bool isUpgrade)
        {
            if (!_windowService.IsOpen(WindowType.Tutorial))
            {
                await _windowService.Open(WindowType.Tutorial);
            }

            HighlightUpgradeButton?.Invoke(isUpgrade);
        }

        public async void ShowHighlightHudButton(WindowType type)
        {
            if (!_windowService.IsOpen(WindowType.Tutorial))
            {
                await _windowService.Open(WindowType.Tutorial);
            }

            HighlightHudButton?.Invoke(type);
        }

        private void EnterState()
        {
            switch (StageType)
            {
                case TutorialStageType.Welcome:
                    _stateMachine.Enter<WelcomeTutorialState>();
                    break;
                case TutorialStageType.ShowMiningOil:
                    _stateMachine.Enter<CreateMiningOilTutorialState>();
                    break;
                case TutorialStageType.CreateComplex:
                    _stateMachine.Enter<CreateComplexTutorialState>();
                    break;
                case TutorialStageType.ShowProcessing:
                    _stateMachine.Enter<CreateProcessingState>();
                    break;
                case TutorialStageType.ShowFirstOrder:
                    _stateMachine.Enter<ShowOrderState>();
                    break;
                case TutorialStageType.ShowQuests:
                    _stateMachine.Enter<ShowQuestsState>();
                    break;
                case TutorialStageType.UpgradeComplex:
                    _stateMachine.Enter<UpgradeComplexState>();
                    break;
                case TutorialStageType.CreatedProcessing:
                    _stateMachine.Enter<UpgradeProcessingState>();
                    break;
                case TutorialStageType.LastMessage:
                    _stateMachine.Enter<LastMessageTutorialState>();
                    break;
                case TutorialStageType.Completed:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnClickTutorialFinger()
        {
            ClickFinger?.Invoke();
        }

        public void UpgradeBuild()
        {
            UpgradeClick?.Invoke();
        }
    }
}