using System;
using System.Collections.Generic;
using Infrastructure.Purchasing;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Purchasing;
using Product = Gameplay.Store.Product;

namespace Gameplay.Shop
{
    [Serializable]
    public class PurchaseProductData 
    {
        public ProductType ProductType;
        public AssetReferenceSprite Sprite;
        public string Name;

        // purchase data
        [SerializeField] private PurchaseStaticData purchaseData;
        public PurchaseStaticData PurchaseData => purchaseData;

        private UnityEngine.Purchasing.Product _product;

        public void SetProduct(UnityEngine.Purchasing.Product product)
        {
            _product = product;
        }
        
        public string PriceString => purchaseData == null 
            ? "" 
            : purchaseData.IsInApp 
                ? _product?.metadata?.localizedPriceString ?? "" 
                : purchaseData.Price[0].Amount.ToString();
    }
}