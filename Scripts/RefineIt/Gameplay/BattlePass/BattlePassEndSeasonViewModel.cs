using System;
using System.Threading.Tasks;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.BattlePass
{
    public class BattlePassEndSeasonViewModel : ViewModelBase<BattlePassEndSeasonModel, BattlePassEndSeasonView>
    {
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public BattlePassEndSeasonViewModel(BattlePassEndSeasonModel model, BattlePassEndSeasonView view,
            IProgressService progressService, IStaticDataService staticDataService)
            : base(model, view)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
        }

        public override Task Show()
        {
            var progress = _progressService.PlayerProgress.BattlePassProgressData;
            var config = _staticDataService.BattlePassConfig;
            var experience = progress.Level * config.ExperienceForLevel + progress.Experience;
            
            View.SetData(progress.Level, experience);
            
            return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.GetRewards += OnGetRewards;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            
            View.GetRewards -= OnGetRewards;
        }

        private void OnGetRewards()
        {
            Model.GetRewards();
        }
    }
}