using Infrastructure.StaticData.ProcessingWorkspace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Petrol", menuName = "RemoteLink/Processing/Petrol", order = 0)]
    public class ProcessingPetrolConfigLinkRemote : ScriptableObject
    {
        [SerializeField] private ProcessingPetrolRemoteConfig _remoteConfig;
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

                levelConfig.ProduceStorageCapacity[0].Capacity = remoteConfig.StorageProduceResource;

                levelConfig.RequiredStorageCapacity[0].Capacity = remoteConfig.StorageRequiredResource;
                levelConfig.ResourceConversionData.OutputResources[0].Value = remoteConfig.ProduceResource;

                levelConfig.ResourceConversionData.InputResources[0].Value = remoteConfig.RequiredResource;
                levelConfig.ProcessingTime = remoteConfig.ProcessingTime;
            }
        }
#endif
    }
}