using System;
using System.Threading.Tasks;
using Gameplay.Investing;
using Gameplay.Quests;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Offer.UI
{
    public class OfferViewModel : ViewModelBase<IOfferModel, OfferView>
    {
        private OfferProgress _data;
        private OfferStaticData _staticData;
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticDataService;

        public OfferViewModel(IOfferModel model, OfferView view, IAssetProvider assetProvider, IStaticDataService staticDataService) : base(model, view)
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
            View.GetBuyButton().onClick.AddListener(ButtonBuyClicked);
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            View.GetBuyButton().onClick.RemoveListener(ButtonBuyClicked);
        }
        
        private void ButtonBuyClicked()
        {
            var staticData = _staticDataService.OfferData.Offers.Find(x => x.Type == _data.StatusType);
            Model.BuyOffer(staticData.purchaseData.ID);
            UpdateState();
        }

        private void InitializeView()
        {
            _data = Model.GetProgressData();
            UpdateState();
        }

        private void UpdateState()
        {
            var staticData = _staticDataService.OfferData.Offers.Find(x => x.Type == _data.StatusType);
            View.SetIcon(staticData.Sprite);
            View.SetTitle(staticData.Title);
            View.SetDescription(staticData.Description);
            View.SetTextPrice(staticData.PriceString);
            SpawnOfferRewards();
        }

        private void SpawnOfferRewards()
        {
            View.ClearRewardsContainer();
            var staticData = _staticDataService.OfferData.Offers.Find(x => x.Type == _data.StatusType);
            foreach (var reward in staticData.purchaseData.Currencies)
            {
                var sprite = _assetProvider.LoadSprite(_staticDataService.GetCurrencyData(reward.Type).Sprite).Result;
                View.SpawnCurrency(sprite, reward.Amount.ToString());
            }
        }
    }
}