using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Cysharp.Threading.Tasks;
using Infrastructure.StaticData;
using JetBrains.Annotations;
using Unity.Services.Core;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Utils;

namespace Infrastructure.Purchasing
{
    [UsedImplicitly]
    public class UnityInAppPurchasingService : IInAppPurchasingService, IDetailedStoreListener
    {
        private readonly IStaticDataService _staticDataService;
        private IStoreController _storeController;
        // ReSharper disable once InconsistentNaming
        private UniTaskCompletionSource<bool> _initializeTaskCS;
        private bool _initializeTaskResult;
        // ReSharper disable once InconsistentNaming
        private UniTaskCompletionSource<bool> _purchaseTaskCS;
        private bool _purchaseTaskResult;
        
        private UnityInAppPurchasingService(
            IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void Initialize()
        {
            InitializePurchasing();
        }
        
        private async UniTask EnsureInitialized()
        {
            if (IsPurchasingInitialized())
            {
                return;
            }

            if (_initializeTaskCS != null)
            {
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASING_INITIALIZATION_IN_PROGRESS);
            }
            
            await InitializePurchasing();

            if (!IsPurchasingInitialized())
            {
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASING_NOT_INITIALIZED);
            }
        }
        
        #region IInAppPurchaseService
        
        public async UniTask<bool> InitializePurchasing()
        {
            if (_initializeTaskCS != null)
            {
                // NOTE: potential exception if Purchase called before Initialization is complete
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASING_INITIALIZATION_IN_PROGRESS);
                //return _initializeTaskCS.Task;
            }
            
            await UnityServices.InitializeAsync();
            
            var inApps = _staticDataService.GetInAppPurchases().ToArray()
                .Where(x => x.IsInApp);
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var inApp in inApps)
            {
                builder.AddProduct(inApp.ID, ProductType.Consumable);
            }
            
            _initializeTaskCS = new UniTaskCompletionSource<bool>();
            UnityPurchasing.Initialize(this, builder);
            
            return await (_initializeTaskCS?.Task ?? UniTask.FromResult(_initializeTaskResult));
        }

        public bool IsPurchasingInitialized()
        {
            var isInitialized = _storeController != null;
            //this.LogWarning($"isInitialized: {isInitialized}");
            return isInitialized;
        }

        public event Action<bool> OnPurchasingInitialize;

        public async UniTask<IEnumerable<Purchase>> GetPurchases()
        {
            await EnsureInitialized();
            
            return from product in _storeController.products.all
                select new Purchase(product, _staticDataService.GetPurchase(product.definition.id));
        }

        public UniTask<bool> Purchase(string title)
        {
            return Purchase(_staticDataService.GetPurchase(title));
        }

        public async UniTask<bool> Purchase(PurchaseStaticData purchase)
        {
            await EnsureInitialized();
            
            if (!purchase.IsInApp)
            {
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASE_NOT_IN_APP);
            }
            
            this.LogWarning($"purchase: {purchase.ID}");

            // perform in-app logic
            if (_purchaseTaskCS != null)
            {
                throw new PurchaseUnsuccessfulException(Exceptions.PURCHASE_IN_PROGRESS);
            }
            
            _purchaseTaskCS = new UniTaskCompletionSource<bool>();
            _storeController.InitiatePurchase(purchase.ID);
            
            return await (_purchaseTaskCS?.Task ?? UniTask.FromResult(_purchaseTaskResult));
        }

        public event Action<PurchaseResult> OnPurchase;

        #endregion
        
        #region IStoreListener

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.Log("in-app purchasing successfully initialized");
            _storeController = controller;

            SetInitializeResult(true);
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            this.LogWarning($"error: {error}");

            SetInitializeResult(false);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            this.LogWarning($"error: {error} message: {message}");

            SetInitializeResult(false);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            //this.LogWarning($"product: {product.definition.id}");

            SetPurchaseResult(true, product);
            
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            this.LogError($"product: {product.definition.id} reason: {failureReason}");

            SetPurchaseResult(false, product);
            
            throw new PurchaseUnsuccessfulException(Exceptions.PURCHASE_UNSUCCESSFUL_WITH_REASON);
            
            //var result = new PurchaseResult(false, _staticDataService.GetPurchase(product.definition.id));
            //OnPurchased?.Invoke(this, result);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            this.LogError($"product: {product.definition.id} description: {failureDescription}");

            SetPurchaseResult(false, product);
            
            throw new PurchaseUnsuccessfulException(Exceptions.PURCHASE_UNSUCCESSFUL_WITH_REASON);
            
            //var result = new PurchaseResult(false, _staticDataService.GetPurchase(product.definition.id));
            //OnPurchased?.Invoke(this, result);
        }

        #endregion

        private void SetInitializeResult(bool isSuccess)
        {
            _initializeTaskResult = isSuccess;
            _initializeTaskCS?.TrySetResult(_initializeTaskResult);
            _initializeTaskCS = null;
            
            //this.LogWarning($"isSuccess: {isSuccess}");
            
            OnPurchasingInitialize?.Invoke(_initializeTaskResult);
        }
        
        private void SetPurchaseResult(bool isSuccess, Product product)
        {
            _purchaseTaskResult = isSuccess;
            _purchaseTaskCS?.TrySetResult(_purchaseTaskResult);
            _purchaseTaskCS = null;
            
            var result = new PurchaseResult(_purchaseTaskResult, _staticDataService.GetPurchase(product.definition.id));
            OnPurchase?.Invoke(result);
        }
    }
}