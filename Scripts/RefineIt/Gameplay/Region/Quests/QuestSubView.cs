using System;
using Gameplay.Currencies;
using Gameplay.Orders;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Gameplay.Orders;
using Gameplay.Region.Quests;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Zenject;

namespace Gameplay.Region.Storage
{
    public class QuestSubView : SubView<QuestSubData>
    {
        [SerializeField] private Image _imageDaily;
        [SerializeField] private TMP_Text _textDescription;
        [SerializeField] private TMP_Text _textReward;
        [SerializeField] private TakeDailyRewardButton _buttonTake;
        [SerializeField] private RefreshDailyButton _buttonRefresh;
        [SerializeField] private Slider _questProgress;
        [SerializeField] private TMP_Text _questProgressText;
        [SerializeField] private Image _rewardImage;
        private int count;
        private int progress;
        
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticDataService;

        public TakeDailyRewardButton ButtonTake => _buttonTake;
        public RefreshDailyButton ButtonAds => _buttonRefresh;

        [Inject]
        public void Construct(IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public override void Initialize(QuestSubData data)
        {
            count = data.Count;
            progress = data.Progress;
            var sliderValue = (float)progress / count;
            
            _imageDaily.sprite = data.Sprite;
            _textDescription.text = data.Description;
            _textReward.text = data.Reward.ToFormattedBigNumber();
            _questProgress.value = sliderValue;
            _questProgressText.text = $"{progress}/{count}";
            _buttonTake.Initialize(data.Reward, data.Guid);
            _buttonRefresh.Initialize(data.Guid, data.IsDaily);
            _rewardImage.sprite = data.RewardSprite;
        }

        public void ActiveRewardButton(bool isActive)
        {
            _buttonTake.gameObject.GetComponent<Button>().interactable = isActive;
        }
        
        public void HideRewardButton()
        {
            _buttonTake.transform.parent.gameObject.SetActive(false);
        }
        
        public void HideRefreshButton()
        {
            _buttonRefresh.gameObject.SetActive(false);
        }

        public GameObject GetRefreshGameObject()
        {
            return _buttonRefresh.gameObject;
        }
    }
}