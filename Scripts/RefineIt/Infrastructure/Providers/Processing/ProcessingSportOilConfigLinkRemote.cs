using Infrastructure.StaticData.ProcessingWorkspace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "SportOil", menuName = "RemoteLink/Processing/SportOil", order = 0)]
    public class ProcessingSportOilConfigLinkRemote : ScriptableObject
    {
        [SerializeField] private ProcessingSportOilRemoteConfig _remoteConfig;
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
                levelConfig.RequiredStorageCapacity[0].Capacity = remoteConfig.StorageRequiredResource1;
                levelConfig.RequiredStorageCapacity[1].Capacity = remoteConfig.StorageRequiredResource2;
                levelConfig.ResourceConversionData.OutputResources[0].Value = remoteConfig.ProduceResource;
         
                levelConfig.ResourceConversionData.InputResources[0].Value = remoteConfig.RequiredResource1;
                levelConfig.ResourceConversionData.InputResources[1].Value = remoteConfig.RequiredResource2;
                levelConfig.ProcessingTime = remoteConfig.ProcessingTime;
            }
        }
#endif
    }
}