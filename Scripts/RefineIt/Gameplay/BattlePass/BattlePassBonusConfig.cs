using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.BattlePass
{
    [CreateAssetMenu(fileName = "BattlePassBonusConfig", menuName = "Configs/BattlePassBonusConfig", order = 0)]
    public class BattlePassBonusConfig : ScriptableObject
    {
        [SerializeField] private List<BattlePassBonusData> bonuses = new();

        public List<BattlePassBonusData> Bonuses => bonuses;
    }
}