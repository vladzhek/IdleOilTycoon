using Gameplay.Settings;
using Gameplay.Settings.UI;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Investing.UI
{
    public class InvestingViewModelFactory : IViewModelFactory<InvestingViewModel, InvestingView, IInvestingModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;

        public InvestingViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public InvestingViewModel Create(IInvestingModel model, InvestingView view) => 
            new InvestingViewModel(model, view, _assetProvider,_staticDataService);
    }
}