using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "HardResources", menuName = "RemoteConfigs/Complex/HardResources", order = 0)]
    public class ComplexHardResourcesRemoteConfig : AbstractConfigProvider
    {
        [PageName("ComplexHardResources")] public List<ComplexRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}