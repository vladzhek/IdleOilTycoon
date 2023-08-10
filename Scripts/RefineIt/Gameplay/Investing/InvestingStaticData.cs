using Gameplay.Currencies;
using UnityEngine;

namespace Gameplay.Quests
{
    [CreateAssetMenu(fileName = "Investing", menuName = "Data/Investing")]
    public class InvestingStaticData : ScriptableObject
    {
        public int InvestingHardCurrency;
        public int CountBoost;
    }
}