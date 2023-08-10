using System.Collections.Generic;
using Infrastructure.Purchasing;
using UnityEngine;

namespace Gameplay.MoneyBox
{
    [CreateAssetMenu(fileName = "MoneyBoxConfig", menuName = "Configs/MoneyBoxConfig", order = 0)]
    public class MoneyBoxConfig : ScriptableObject
    {
        [SerializeField] private PurchaseStaticData _purchaseData;
        
        public List<MoneyBoxData> MoneyBoxesData;
        
        public PurchaseStaticData PurchaseData => _purchaseData;

        private UnityEngine.Purchasing.Product _product;
        
        public void SetProduct(UnityEngine.Purchasing.Product product)
        {
            _product = product;
        }
        
        public string PriceString => _purchaseData == null 
            ? "" 
            : _purchaseData.IsInApp 
                ? _product?.metadata?.localizedPriceString ?? "" 
                : _purchaseData.Price[0].Amount.ToString();
    }
}
