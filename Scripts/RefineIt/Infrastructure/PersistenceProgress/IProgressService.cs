using System;
using ModestTree.Util;

namespace Infrastructure.PersistenceProgress
{
    public interface IProgressService
    {
        PlayerProgress PlayerProgress { get; }
        RegionProgress RegionProgress { get; }
        bool IsLoaded { get; }
        event Action OnLoaded; 
        void InitializeProgress(PlayerProgress playerProgress);
    }
}