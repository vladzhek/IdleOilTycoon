using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.BattlePass
{
    [CreateAssetMenu(fileName = "BattlePassRewardConfig", menuName = "Configs/BattlePassRewardConfig", order = 0)]
    public class BattlePassRewardConfig : ScriptableObject
    {
        [SerializeField] private List<BattlePassRewardData> _rewards = new();

        public List<BattlePassRewardData> Rewards => _rewards;
    }
}