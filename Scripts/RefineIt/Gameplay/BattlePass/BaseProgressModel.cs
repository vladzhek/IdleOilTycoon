using System;
using Infrastructure.PersistenceProgress;
using Zenject;

namespace Gameplay.BattlePass
{
    public abstract class BaseProgressModel : IInitializable, IDisposable
    {
        public virtual void Initialize()
        {
            if (ProgressModel.IsLoaded)
            {
                OnProgressLoaded();
            }
            else
            {
                ProgressModel.OnLoaded += OnProgressLoaded;
            }
        }

        public virtual void Dispose()
        {
            
        }
        
        // private

        protected IProgressService ProgressModel { get; }

        protected BaseProgressModel(IProgressService progressModel)
        {
            ProgressModel = progressModel;
        }

        protected virtual void OnProgressLoaded()
        {
            ProgressModel.OnLoaded -= OnProgressLoaded;
        }
        
        protected PlayerProgress PlayerProgressData
        {
            get
            {
                var gameData = ProgressModel.PlayerProgress;
                if (gameData == null)
                {
                    throw new Exception("can't retrieve game data");
                }
                return gameData;
            }
        }
    }
}