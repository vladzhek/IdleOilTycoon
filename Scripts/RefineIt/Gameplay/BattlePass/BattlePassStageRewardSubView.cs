using System;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;

namespace Gameplay.BattlePass
{
    public class BattlePassStageRewardSubView : SubView<BattlePassStageRewardSubViewData>
    {
        public event Action<BattlePassRewardSubViewData, int> TakeReward;
        public event Action<BattlePassRewardSubViewData> RewardInfoClick;

        [SerializeField] private BattlePassRewardSubView _freeBattlePassRewardSubView;
        [SerializeField] private BattlePassRewardSubView _premiumBattlePassRewardSubView;

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _levelLock;

        [SerializeField] private GameObject _lockBackGround;
        
        private BattlePassStageRewardSubViewData _data;

        public override void Initialize(BattlePassStageRewardSubViewData data)
        {
            _data = data;

            _freeBattlePassRewardSubView.Initialize(data.FreeReward, data.IsBuyBattlePass, true,
                data.IsOpen);
            _premiumBattlePassRewardSubView.Initialize(data.PremiumReward, data.IsBuyBattlePass, false,
                data.IsOpen);

            _levelText.text = $"{data.Level + 1}";
            _levelLock.text = $"{_data.Level + 1}";
            
            _levelLock.transform.parent.parent.gameObject.SetActive(!data.IsOpen);
            _levelText.transform.parent.parent.gameObject.SetActive(data.IsOpen);
        }
        
        

        private void OnEnable()
        {
            _freeBattlePassRewardSubView.TakeReward += TakeRewardClick;
            _premiumBattlePassRewardSubView.TakeReward += TakeRewardClick;
            _premiumBattlePassRewardSubView.RewardsInfoClick += OnRewardsInfoClick;
            _freeBattlePassRewardSubView.RewardsInfoClick += OnRewardsInfoClick;
        }

        private void OnDisable()
        {
            _freeBattlePassRewardSubView.TakeReward -= TakeRewardClick;
            _premiumBattlePassRewardSubView.TakeReward -= TakeRewardClick;
            _premiumBattlePassRewardSubView.RewardsInfoClick -= OnRewardsInfoClick;
            _freeBattlePassRewardSubView.RewardsInfoClick -= OnRewardsInfoClick;
        }

        private void OnRewardsInfoClick(BattlePassRewardSubViewData data)
        {
            RewardInfoClick?.Invoke(data);
        }

        private void TakeRewardClick(BattlePassRewardSubViewData rewardData)
        {
            TakeReward?.Invoke(rewardData, _data.Level);
        }

        public void SetOpen(bool IsOpen)
        {
            _lockBackGround.SetActive(!IsOpen);
        }
    }
}