using System;

namespace Infrastructure.PersistenceProgress
{
    public class PlayerProgressService : IProgressService
    {
        public PlayerProgress PlayerProgress { get; private set; }
        public RegionProgress RegionProgress => PlayerProgress.GetRegionProgress();
        public bool IsLoaded { get; set; } = false;
        public event Action OnLoaded;

        public void InitializeProgress(PlayerProgress playerProgress)
        {
            PlayerProgress = playerProgress;

            IsLoaded = true;
            OnLoaded?.Invoke();
        }
    }
}