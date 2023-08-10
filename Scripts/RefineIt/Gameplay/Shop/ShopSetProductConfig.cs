using System.Collections.Generic;
using Gameplay.Store;
using UnityEngine;

namespace Gameplay.Shop
{
    [CreateAssetMenu(fileName = "ShopSetProductConfig", menuName = "Configs/ShopSetProductConfig", order = 0)]
    public class ShopSetProductConfig : ScriptableObject
    {
        public Sprite Background;
        public Sprite CounterBackground;
        
        public List<PurchaseProductData> SetProducts = new();
    }
}