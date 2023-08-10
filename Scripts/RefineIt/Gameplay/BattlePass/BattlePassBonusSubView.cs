using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.BattlePass
{
    public class BattlePassBonusSubView : SubView<BattlePassBonusSubViewData>
    {
        [SerializeField] private Image _bonusImage;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private GameObject _lock;
        
        public override void Initialize(BattlePassBonusSubViewData data)
        {
            _bonusImage.sprite = data.BonusSprite;
            _value.text = data.Value;
            _description.text = data.Description;
            
            _lock.SetActive(!data.IsBuy);
        }

        public void SetLock(bool isActive)
        {
            _lock.SetActive(isActive);
        }
    }
}