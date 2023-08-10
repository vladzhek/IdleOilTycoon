using System;
using Infrastructure.PersistenceProgress;

namespace Gameplay.Shop
{
    public class PurchaseProductModel
    {
        public event Action<PurchaseProductModel> BuyClick;
        
        public PurchaseProductModel(ShopPurchaseProductProgress shopCurrencyProductProgress, PurchaseProductData data,
            ShopProductConfig shopProductConfig)
        {
            ShopCurrencyProductProgress = shopCurrencyProductProgress;
            Data = data;
            Config = shopProductConfig;
        }

        public ShopPurchaseProductProgress ShopCurrencyProductProgress { get; private set; }
        public PurchaseProductData Data { get; private set; }
        public ShopProductConfig Config { get; private set; }

        public void Click()
        {
            BuyClick?.Invoke(this);
        }
    }
}