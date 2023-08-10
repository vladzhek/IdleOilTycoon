using System.Threading.Tasks;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Utils.Extensions;

namespace Gameplay.BattlePass
{
    public class BattlePassBonusViewModel : ViewModelBase<BattlePassBonusModel, BattlePassBonusView>
    {
        private readonly BattlePassConfig _config;
        private readonly BattlePassModel _battlePassModel;
        private readonly IProgressService _progressService;

        public BattlePassBonusViewModel(BattlePassBonusModel model, BattlePassBonusView view,
            IStaticDataService staticDataService, BattlePassModel battlePassModel, IProgressService progressService) : base(model, view)
        {
            _battlePassModel = battlePassModel;
            _progressService = progressService;
            _config = staticDataService.BattlePassConfig;
        }

        public override Task Show()
        {
            InitializeBonusContainer();
            UpdateExperience();
            
            return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            
            _battlePassModel.Timer.Tick += OnTimerTick;
            _battlePassModel.OnBuyBattlePass += BuyBattlePass;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            _battlePassModel.Timer.Tick -= OnTimerTick;
            _battlePassModel.OnBuyBattlePass -= BuyBattlePass;
        }

        private void BuyBattlePass()
        {
            foreach (var subView  in View.BattlePassBonusSubViewContainer.SubViews.Values)
            {
                subView.SetLock(false);
            }
        }

        private void UpdateExperience()
        {
            var progress = _progressService.PlayerProgress.BattlePassProgressData;
            View.SetLevel(progress.Level);
            View.SetExperienceSlider(_config.ExperienceForLevel, progress.Experience);
        }
        
        private void OnTimerTick(int time)
        {
            View.SetTimer(FormatTime.DayAndHoursToString(time));
        }

        private void InitializeBonusContainer()
        {
            foreach (BattlePassBonusData booster in _config.BonusesConfig.Bonuses)
            {
                var data = new BattlePassBonusSubViewData
                {
                    BonusSprite = booster.BonusSprite,
                    Description = booster.Description,
                    IsBuy = _progressService.PlayerProgress.BattlePassProgressData.IsBuy,
                    Type = booster.Type,
                };

                if (booster.Type == BattlePassBonusType.TimeRecycling)
                {
                    data.Value = $"-{booster.Value}%";
                }
                else
                {
                    data.Value = $"+{booster.Value}%";
                }
                
                View.BattlePassBonusSubViewContainer.Add(booster.Type.ToString(), data);
            }
        }
    }
}