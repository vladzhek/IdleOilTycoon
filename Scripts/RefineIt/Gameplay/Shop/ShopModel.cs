using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Infrastructure.PersistenceProgress;
using Infrastructure.Purchasing;
using Infrastructure.StaticData;
using Utils;

namespace Gameplay.Shop
{
    public class ShopModel : IShopModel
    {
        public List<PurchaseProductModel> CurrenciesProducts { get; } = new();
        public List<PurchaseProductModel> WorkerProducts { get; } = new();
        public List<PurchaseProductModel> HardProducts { get; } = new();
        public List<PurchaseProductModel> SetProducts { get; } = new();

        public event Action OnHardProductsChange;
        public event Action OnSetProductsChange;
        public event Action OnSoftProductsChange;
        public event Action OnWorkerProductsChange;

        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        private readonly IGenericPurchasingService _purchasingService;
        private readonly CurrenciesModel _currenciesModel;

        public ShopModel(IStaticDataService staticDataService,
            IProgressService progressService,
            IGenericPurchasingService purchasingService,
            CurrenciesModel currenciesModel)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _purchasingService = purchasingService;
            _currenciesModel = currenciesModel;
        }

        public void Initialize()
        {
            CreateSoftProducts();
            CreateWorkerProducts();
            CreateSetProducts();
            CreateHardProducts();

            /*if (_purchasingService.IsPurchasingInitialized())
            {
               
            }
            else
            {
                this.LogWarning($"purchasing not initialized, subscribing...");

                _purchasingService.OnPurchasingInitialize += OnPurchasingInitialized;
            }*/
        }

        private void OnPurchasingInitialized(bool isInitialized)
        {
            this.LogWarning($"isInitialized: {isInitialized}");

            if (isInitialized)
            {
                _purchasingService.OnPurchasingInitialize -= OnPurchasingInitialized;

                CreateSetProducts();
                CreateHardProducts();
            }
            else
            {
                // TODO: purchasing not initialized, manual restart, indication etc
            }
        }

        private void CreateSoftProducts()
        {
            var products = _staticDataService.ShopConfigData.SoftProducts;
            CreateCurrencyData(products, CurrenciesProducts);
            OnSoftProductsChange?.Invoke();
        }

        private void CreateWorkerProducts()
        {
            var co = _staticDataService.ShopConfigData.WorkerProducts;
            CreateCurrencyData(co, WorkerProducts);
            OnWorkerProductsChange?.Invoke();
        }

        private async void CreateSetProducts()
        {
            var config = _staticDataService.ShopConfigData.SetProducts;
            //  var purchases = await _purchasingService.GetPurchases();
            CreateCurrencyData(config, SetProducts);

            OnSetProductsChange?.Invoke();
        }


        private async void CreateHardProducts()
        {
            var config = _staticDataService.ShopConfigData.HardProducts;
            //    var purchases = await _purchasingService.GetPurchases();
            CreateCurrencyData(config, HardProducts);

            OnHardProductsChange?.Invoke();
        }

        public void CreateCurrencyData(ShopProductConfig config, List<PurchaseProductModel> models)
        {
            try
            {
                models.Clear();
                foreach (var product in config.Products)
                {
                    models.Add(CreatePurchaseProductModel(product.PurchaseData.ID, product, config));
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"{e}");
            }
        }

        private void CreatePurchaseData(IEnumerable<Purchase> purchases,
            ShopProductConfig config, List<PurchaseProductModel> models)
        {
            try
            {
                models.Clear();
                foreach (var purchase in purchases)
                {
                    foreach (var product in config.Products)
                    {
                        if (product.PurchaseData.ID != purchase.Data.ID)
                        {
                            continue;
                        }

                        //this.LogWarning($"id: {purchase.Data.ID}");

                        // product.SetProduct(purchase.Product);
                        models.Add(CreatePurchaseProductModel(purchase.Data.ID, product, config));
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"{e}");
            }
        }

        private PurchaseProductModel CreatePurchaseProductModel(string id, PurchaseProductData data,
            ShopProductConfig config)
        {
            var progress = _progressService.PlayerProgress
                .GetOrCreatePurchaseProductProgress(id);

            var model = new PurchaseProductModel(progress, data, config);

            model.BuyClick += OnPurchaseBuyClick;

            return model;
        }

        private void OnPurchaseBuyClick(PurchaseProductModel data)
        {
            _purchasingService.Purchase(data.Data.PurchaseData.ID);
        }
    }
}