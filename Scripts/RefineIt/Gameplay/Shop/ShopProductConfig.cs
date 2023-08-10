using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Shop
{
    [CreateAssetMenu(fileName = "HardProductData", menuName = "Configs/HardProductData", order = 0)]
    public class ShopProductConfig : ScriptableObject
    {
        public Sprite Background;
        public Sprite CounterBackground;
        
        public List<PurchaseProductData> Products;
    }
}