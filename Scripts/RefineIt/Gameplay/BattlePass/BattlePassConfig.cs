using Gameplay.Shop;
using Infrastructure.Purchasing;
using UnityEngine;

namespace Gameplay.BattlePass
{
    [CreateAssetMenu(fileName = "BattlePassConfig", menuName = "Configs/BattlePassConfig", order = 0)]
    public class BattlePassConfig : ScriptableObject
    {
        [SerializeField] private int _experienceForLevel = 200;

        [SerializeField] private BattlePassRewardConfig _freeRewards;
        [SerializeField] private BattlePassRewardConfig _premiumRewards;
        [SerializeField] private BattlePassBonusConfig bonusesConfig;

        [SerializeField] private PurchaseStaticData _purchaseProductData;
        
        private UnityEngine.Purchasing.Product _product;
        
        public void SetProduct(UnityEngine.Purchasing.Product product)
        {
            _product = product;
        }
        
        public string PriceString => _purchaseProductData == null 
            ? "" 
            : _purchaseProductData.IsInApp 
                ? _product?.metadata?.localizedPriceString ?? "" 
                : _purchaseProductData.Price[0].Amount.ToString();

        public int ExperienceForLevel => _experienceForLevel;
        public BattlePassRewardConfig PremiumRewards => _premiumRewards;
        public BattlePassRewardConfig FreeRewards => _freeRewards;
        public BattlePassBonusConfig BonusesConfig => bonusesConfig;

        public PurchaseStaticData PurchaseData => _purchaseProductData;
    }
}