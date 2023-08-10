using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.RewardPopUp
{
    public class RewardPopupSubView : SubView<RewardData>
    {
        [SerializeField] private Image _resourceSprite;
        [SerializeField] private TextMeshProUGUI _valueText;
        
        public override void Initialize(RewardData data)
        {
            _resourceSprite.sprite = data.RewardSprite;
            _valueText.text = $"{data.RewardQuantity}";
        }
    }
}