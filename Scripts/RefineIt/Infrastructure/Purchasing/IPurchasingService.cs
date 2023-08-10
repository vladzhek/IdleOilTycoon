using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Purchasing;
using Zenject;

namespace Infrastructure.Purchasing
{
    public interface IPurchasingService : IInitializable
    {
        UniTask<bool> InitializePurchasing();
        bool IsPurchasingInitialized();
        public event Action<bool> OnPurchasingInitialize;

        UniTask<IEnumerable<Purchase>> GetPurchases();

        UniTask<bool> Purchase(string title);
        UniTask<bool> Purchase(PurchaseStaticData purchase);
        public event Action<PurchaseResult> OnPurchase;
    }

    public interface IGenericPurchasingService : IPurchasingService
    {
        
    }

    public class PurchaseResult
    {
        public readonly bool IsSuccess;
        public readonly PurchaseStaticData Data;

        public PurchaseResult(bool isSuccess, PurchaseStaticData data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }
    }

    public class Purchase
    {
        public readonly Product Product;
        public readonly PurchaseStaticData Data;
        
        public Purchase(Product product, PurchaseStaticData data)
        {
            Product = product;
            Data = data;
        }
    }
}