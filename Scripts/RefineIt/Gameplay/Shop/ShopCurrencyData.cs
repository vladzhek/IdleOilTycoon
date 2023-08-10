using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Shop
{
    [CreateAssetMenu(fileName = "ShopCurrencyDataData", menuName = "Configs/ShopCurrencyDataData", order = 0)]
    public class ShopCurrencyData : ScriptableObject
    {
        public Sprite Background;
        public Sprite CounterBackground;
        
        public List<PurchaseProductData> Products;
    }
}