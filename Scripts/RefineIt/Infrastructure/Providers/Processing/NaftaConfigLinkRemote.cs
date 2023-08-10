using System;
using Infrastructure.StaticData.ProcessingWorkspace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Nafta", menuName = "RemoteLink/Processing/NaftaConfig", order = 0)]
    public class NaftaConfigLinkRemote : ScriptableObject
    {
        [SerializeField] private NaftaProcessingRemoteConfig _remoteConfig;
        public ProcessingWorkspaceStaticData Config;

#if UNITY_EDITOR
        [Button("SetData"), ShowInInspector]
        public void SetDataInConfig()
        {
            for (int index = 0; index < _remoteConfig.Data.Count; index++)
            {
                var remoteConfig = _remoteConfig.Data[index];

                var levelConfig = Config.BuildingLevels[index];

                levelConfig.UpdateCost = remoteConfig.LevelPrice;

                levelConfig.ProduceStorageCapacity[0].Capacity = remoteConfig.StorageNafta;
                levelConfig.ProduceStorageCapacity[1].Capacity = remoteConfig.StorageVg;
                levelConfig.ProduceStorageCapacity[2].Capacity = remoteConfig.StorageDtvs;
                levelConfig.ProduceStorageCapacity[3].Capacity = remoteConfig.StorageGoodron;

                levelConfig.RequiredStorageCapacity[0].Capacity = remoteConfig.StorageOil;
                levelConfig.ResourceConversionData.OutputResources[0].Value = remoteConfig.ProduceNafta;
                levelConfig.ResourceConversionData.OutputResources[1].Value = remoteConfig.ProduceVg;
                levelConfig.ResourceConversionData.OutputResources[2].Value = remoteConfig.ProduceDtvs;
                levelConfig.ResourceConversionData.OutputResources[3].Value = remoteConfig.ProduceGoodron;


                levelConfig.ResourceConversionData.InputResources[0].Value = remoteConfig.RequiredOil;
            }
        }
#endif
    }
}