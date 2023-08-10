using Gameplay.Workspaces.ComplexWorkspace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Tanker", menuName = "RemoteLink/Complex/Tanker", order = 0)]
    public class ComplexTankerConfigLinkRemote : ScriptableObject
    {
        [SerializeField] private ComplexTankerRemoteConfig _remoteConfig;
        public ComplexWorkspaceStaticData Config;

#if UNITY_EDITOR
        [Button("SetData"), ShowInInspector]
        public void SetDataInConfig()
        {
            for (int index = 0; index < _remoteConfig.Data.Count; index++)
            {
                var remoteConfig = _remoteConfig.Data[index];
                var levelConfig = Config.BuildingLevels[index];
                levelConfig.UpdateCost = remoteConfig.LevelPrice;

                for (int i = 0; i < levelConfig.ResourcesCapacity.Count; i++)
                {
                    var resource = levelConfig.ResourcesCapacity[i];

                    switch (i)
                    {
                        case 0:
                            resource.Capacity = remoteConfig.StorageResource1;
                            break;
                        case 1:
                            resource.Capacity = remoteConfig.StorageResource2;
                            break;
                        case 2:
                            resource.Capacity = remoteConfig.StorageResource3;
                            break;
                        case 3:
                            resource.Capacity = remoteConfig.StorageResource4;
                            break;
                    }
                }
            }
        }
#endif
    }
}