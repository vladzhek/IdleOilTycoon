using System;
using System.Collections.Generic;
using Infrastructure.Purchasing;
using UnityEngine;

namespace Gameplay.Offer
{
    [Serializable]
    public class OfferData
    {
        [SerializeField] public PurchaseStaticData purchaseData;
        public string Title;
        [TextArea] public string Description;
        public Sprite Sprite;
        public OfferType Type;
        
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