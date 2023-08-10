using Gameplay.MoneyBox;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Offer.UI
{
    public class OfferViewModelFactory: IViewModelFactory<OfferViewModel, OfferView, IOfferModel>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public OfferViewModelFactory(IAssetProvider assetProvider ,IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public OfferViewModel Create(IOfferModel model, OfferView view)
        {
            return new OfferViewModel(model, view ,_assetProvider ,_staticDataService);
        }
    }
}