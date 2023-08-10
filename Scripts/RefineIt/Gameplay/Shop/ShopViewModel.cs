using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Offer;
using Gameplay.RewardPopUp;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;

namespace Gameplay.Shop
{
    public class ShopViewModel : ViewModelBase<IShopModel, ShopView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly RewardPopupModel _rewardPopupModel;
        private readonly IOfferModel _offerModel;

        public ShopViewModel(IShopModel model, ShopView view, IAssetProvider assetProvider, 
            IStaticDataService staticDataService, RewardPopupModel rewardPopupModel,
            IOfferModel offerModel) 
            : base(model, view)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _rewardPopupModel = rewardPopupModel;
            _offerModel = offerModel;
        }

        public override Task Show()
        {
            InitializeSetProducts();
            InitializeCurrenciesProducts();
            InitializeOffer();

            return Task.CompletedTask;
        }

        private void InitializeOffer()
        {
            var data = _offerModel.GetProgressData();
            var staticData = _staticDataService.OfferData.Offers.Find(x => x.Type == data.StatusType);
            
            View.OfferPrefab.Initialize(staticData.Sprite, staticData, staticData.PriceString);
        }

        private async void InitializeSetProducts()
        {
            View.SetProductSubViewContainer.CleanUp();
            await CreatePurchaseSubViews(Model.SetProducts, View.SetProductSubViewContainer);
        }

        private async void InitializeCurrenciesProducts()
        {
            View.CurrenciesProductSubViewContainer.CleanUp();
            await CreatePurchaseSubViews(Model.HardProducts, View.CurrenciesProductSubViewContainer);
            await CreatePurchaseSubViews(Model.CurrenciesProducts, View.CurrenciesProductSubViewContainer);
            await CreatePurchaseSubViews(Model.WorkerProducts, View.CurrenciesProductSubViewContainer);
        }
        
        private async Task CreatePurchaseSubViews(List<PurchaseProductModel> purchaseProductModel,
            SubViewContainer<PurchaseSubView, PurchaseViewData> subViewContainer)
        {
            for (int i = 0; i < purchaseProductModel.Count; i++)
            {
                var model = purchaseProductModel[i];
                var data = model.Data;

                string id = data.ProductType.ToString() + i;

                var viewData = new PurchaseViewData
                {
                    PurchaseSprite = await _assetProvider.LoadSprite(data.Sprite),
                    CurrencySprite = await _assetProvider.LoadSprite(_staticDataService.GetCurrencyData
                        (model.Data.PurchaseData.Price[0].Type).Sprite),
                    PriceString = data.PriceString == "" ? "Купить" :data.PriceString,
                    PurchaseProductData = data,
                    Background = model.Config.Background,
                    CounterBackground = model.Config.CounterBackground
                };

                subViewContainer.Add(id, viewData);

                subViewContainer.SubViews[id].Click += model.Click;
                subViewContainer.SubViews[id].InfoClick += OnInfoClick;
            }
        }

        private async void OnInfoClick(PurchaseViewData data)
        {
            var rewardPopupData = new RewardsPopupData();

            foreach (var currency in data.PurchaseProductData.PurchaseData.Currencies)
            {
                var currencySprite = _staticDataService.GetCurrencyData(currency.Type).Sprite;

                rewardPopupData.Rewards.Add(new RewardData
                {
                    RewardQuantity = currency.Amount,
                    RewardSprite = await _assetProvider.LoadSprite(currencySprite)
                });
                
                rewardPopupData.LootBoxSprite = data.PurchaseSprite;
            }

            _rewardPopupModel.ShowRewardPopUp(rewardPopupData);
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Model.OnHardProductsChange += OnHardProductsChanged;
            Model.OnSetProductsChange += OnSetProductsChanged;
            Model.OnSoftProductsChange += OnSoftProductsChanged;
            _offerModel.OnBuyOffer += OnBuyOffer;
        }
        
        public override void Unsubscribe()
        {
            base.Unsubscribe();

            Model.OnSetProductsChange -= OnSetProductsChanged;
            Model.OnHardProductsChange -= OnHardProductsChanged;
            Model.OnSoftProductsChange -= OnSoftProductsChanged;
            _offerModel.OnBuyOffer -= OnBuyOffer;
        }
        
        private void OnBuyOffer()
        {
            InitializeOffer();
        }

        private void OnHardProductsChanged()
        {
            InitializeCurrenciesProducts();
        }

        private void OnSetProductsChanged()
        {
            InitializeSetProducts();
        }
        

        private void OnSoftProductsChanged()
        {
            InitializeCurrenciesProducts();
        }
    }
}