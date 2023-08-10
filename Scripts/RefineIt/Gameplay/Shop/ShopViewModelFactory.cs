using Gameplay.Offer;
using Gameplay.RewardPopUp;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Shop
{
    public class ShopViewModelFactory : IViewModelFactory<ShopViewModel, ShopView, IShopModel>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly RewardPopupModel _rewardPopupModel;
        private readonly IOfferModel _offerModel;

        public ShopViewModelFactory(RewardPopupModel rewardPopupModel, IStaticDataService staticDataService,
            IAssetProvider assetProvider, IOfferModel offerModel)
        {
            _rewardPopupModel = rewardPopupModel;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _offerModel = offerModel;
        }

        public ShopViewModel Create(IShopModel model, ShopView view) =>
            new(model, view, _assetProvider, _staticDataService, _rewardPopupModel, _offerModel);
    }
}