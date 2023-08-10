using System;
using System.Threading.Tasks;
using Gameplay.Currencies;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Investing.UI
{
    public class InvestingViewModel : ViewModelBase<IInvestingModel, InvestingView>
    {
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;
        
        private InvestingProgress _data;
        
        public InvestingViewModel(IInvestingModel model, InvestingView view,
            IAssetProvider assetProvider,
            IStaticDataService staticDataService
        ) : base(model, view)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public override async Task Show()
        {
            InitializeView();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            Model.OnTimerTick += View.UpdateProcessState;
            Model.OnTimerStopped += UpdateState;
            View.InvestingClick += ButtonClicked;
            
            if (Model.GetProgressData().CountBoost <= 0 && _data.StatusType == ViewStatusType.InProgress)
            {
                View.SetDisableButtonADS();
            }
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            Model.OnTimerTick -= View.UpdateProcessState;
            Model.OnTimerStopped -= UpdateState;
            View.InvestingClick -= ButtonClicked;
        }

        private void InitializeView()
        {
            _data = Model.GetProgressData();
            UpdateState();
        }

        private void ButtonClicked()
        {
            switch (_data.StatusType)
            {
                case ViewStatusType.Default:
                    Model.StartInvestingProcess();
                    UpdateState();
                    break;
                case ViewStatusType.InProgress:
                    Model.BoostInvestingProcess();
                    if (Model.GetProgressData().CountBoost <= 0)
                    {
                        View.SetDisableButtonADS();
                    }
                    break;
                case ViewStatusType.ReadyToTake:
                    Model.GetInvestingReward();
                    UpdateState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateState()
        {
            var spriteSoft = _assetProvider.LoadSprite(_staticDataService.GetCurrencyData(CurrencyType.SoftCurrency).Sprite);
            var spriteHard = _assetProvider.LoadSprite(_staticDataService.GetCurrencyData(CurrencyType.HardCurrency).Sprite);
            var amountSoft = Model.GetAmountForReward();
            var amountHard = Model.GetStaticData().InvestingHardCurrency;
            switch (_data.StatusType)
            {
                case ViewStatusType.Default:
                    View.SetDefaultState(spriteHard.Result, amountHard, amountSoft );
                    break;
                case ViewStatusType.InProgress:
                    View.SetProcessState(spriteSoft.Result, amountSoft);
                    View.UpdateProcessState(_data.Timer);
                    break;
                case ViewStatusType.ReadyToTake:
                    View.SetReadyToTakeState(amountSoft);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}