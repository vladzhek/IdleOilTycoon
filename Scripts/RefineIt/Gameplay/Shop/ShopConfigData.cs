using UnityEngine;

namespace Gameplay.Shop
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "Configs/ShopConfig", order = 0)]
    public class ShopConfigData : ScriptableObject
    {
        public ShopProductConfig SoftProducts;
        public ShopProductConfig WorkerProducts;
        public ShopProductConfig HardProducts;
        public ShopProductConfig SetProducts;
    }
}