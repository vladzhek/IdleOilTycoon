using System;
using Gameplay.Store;
using Infrastructure.Purchasing;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Shop
{
    [Serializable]
    public class CurrencyProductData
    {
        public ProductType ProductType;
        public string Name;
        public AssetReferenceSprite Sprite;
        
        // purchase data
        [SerializeField] private PurchaseStaticData purchaseData;
        public PurchaseStaticData PurchaseData => purchaseData;

        private UnityEngine.Purchasing.Product _product;

        public void SetProduct(UnityEngine.Purchasing.Product product)
        {
            _product = product;
        }
    }
}