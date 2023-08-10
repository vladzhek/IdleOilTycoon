using System;
using Gameplay.Currencies;
using Gameplay.Region.Storage;
using Gameplay.Workspaces;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.BattlePass
{
    public class BattlePassBonusModel
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IRegionStorage _regionStorage;
        private readonly IProgressService _progressService;
        private readonly CurrenciesModel _currenciesModel;
        private readonly ITransportSpawner _transportSpawner;
        private readonly IWorkspaceService _workspaceService;
        
        private BattlePassBonusConfig _config;

        public BattlePassBonusModel(IProgressService progressService, IStaticDataService staticDataService,
            IWorkspaceService workspaceService, IRegionStorage regionStorage, CurrenciesModel currenciesModel,
            ITransportSpawner transportSpawner)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _workspaceService = workspaceService;
            _regionStorage = regionStorage;
            _currenciesModel = currenciesModel;
            _transportSpawner = transportSpawner;
        }

        public void Initialize()
        {
            if (_progressService.PlayerProgress.BattlePassProgressData.IsBuy)
            {
                _config = _staticDataService.BattlePassConfig.BonusesConfig;
                InitializeBonus();
            }
        }

        private void InitializeBonus()
        {
            foreach (var bonus in _config.Bonuses)
            {
                switch (bonus.Type)
                {
                    case BattlePassBonusType.ResourceRecycling:
                        AddProcessingBonus(bonus);
                        break;
                    case BattlePassBonusType.MiningOil:
                        MiningOilBonus(bonus);
                        break;
                    case BattlePassBonusType.SellingResource:
                        SellingResourcesBonus();
                        break;
                    case BattlePassBonusType.TimeRecycling:
                        ReduceTimeRecycling(bonus);
                        break;
                    case BattlePassBonusType.TransferSpeed:
                        ReduceTransferSpeed(bonus);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void MiningOilBonus(BattlePassBonusData battlePassBonusData)
        {
            foreach (var model in _workspaceService.MiningWorkspaces.Values)
            {
                var data = _staticDataService.MiningWorkspaceStaticData;
                if (data.ResourceType == ResourceType.Oil)
                {
                    model.AddMiningBonus(data.ResourceType, battlePassBonusData.Value);
                }
            }
        }

        private void AddProcessingBonus(BattlePassBonusData bonus)
        {
            foreach (var model in _workspaceService.ProcessingWorkspaces.Values)
            {
                foreach (var resource in model.CurrentLevelData.ResourceConversionData.OutputResources)
                {
                    model.AddProcessingBonus(resource.ResourceType, bonus.Value);
                }
            }
        }

        private void SellingResourcesBonus()
        {
            _regionStorage.SellResources += OnSellResources;
        }

        private void OnSellResources(ResourceType type, int value)
        {
            var bonusValue = _config.Bonuses
                .Find(x => x.Type == BattlePassBonusType.SellingResource).Value;

            var bonusForSale = value * (bonusValue / 100);
            _currenciesModel.Add(CurrencyType.SoftCurrency, bonusForSale);
        }

        private void ReduceTimeRecycling(BattlePassBonusData bonus)
        {
            foreach (var processing in _workspaceService.ProcessingWorkspaces.Values)
            {
                var bonusValue = processing.CurrentLevelData.ProcessingTime * (bonus.Value / 100);
                processing.CurrentLevelData.ProcessingTime -= bonusValue;
            }
        }

        private void ReduceTransferSpeed(BattlePassBonusData data)
        {
            foreach (var transport in _transportSpawner.Transports)
            {
                var bonusValue = transport.ExportTime * ((float)data.Value / 100);
                transport.ReduceTransferTime(bonusValue);
            }
        }
    }
}