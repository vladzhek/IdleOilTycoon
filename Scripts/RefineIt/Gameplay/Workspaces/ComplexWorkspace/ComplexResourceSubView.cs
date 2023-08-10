using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    public class ComplexResourceSubView : SubView<ComplexResourceData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _resourceName;
        [SerializeField] private TextMeshProUGUI _resourceSliderValue;
        [SerializeField] private TextMeshProUGUI _resourceCapacity;
        [SerializeField] private Slider _slider;

        private int _capacity;

        public void ViewUpdateCapacity(int value, int capacity)
        {
            _resourceSliderValue.text = $"{value.ToFormattedBigNumber()}/{capacity.ToFormattedBigNumber()}";
            _slider.maxValue = capacity;
            _slider.value = value;
            _capacity = capacity;
        }

        public override void Initialize(ComplexResourceData data)
        {
            _capacity = data.ResourceCapacity;
            _image.sprite = data.ResourceSprite;
            _resourceName.text = data.ResourceName.ToString();
            _resourceCapacity.text = data.ResourceCapacity.ToFormattedBigNumber();

            SliderValueView(data);
        }

        private void SliderValueView(ComplexResourceData data)
        {
            _slider.maxValue = data.ResourceCapacity;
            _slider.value = data.Value;
            _resourceSliderValue.text = $"{data.Value.ToFormattedBigNumber()}/{data.ResourceCapacity.ToFormattedBigNumber()}";
        }

        public void UpdateSliderValue(int value)
        {
            _resourceSliderValue.text = $"{value.ToFormattedBigNumber()}/{_capacity.ToFormattedBigNumber()}";
            _slider.value = value;
        }
    }
}