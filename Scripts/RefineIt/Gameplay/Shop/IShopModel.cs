using System;
using System.Collections.Generic;

namespace Gameplay.Shop
{
    public interface IShopModel
    {
        void Initialize();
        
        List<PurchaseProductModel> CurrenciesProducts { get; }
        List<PurchaseProductModel> WorkerProducts { get; }
        List<PurchaseProductModel> HardProducts { get; }
        List<PurchaseProductModel> SetProducts { get; }

        public event Action OnHardProductsChange;
        event Action OnSetProductsChange;
        event Action OnSoftProductsChange;
        event Action OnWorkerProductsChange;
    }
}