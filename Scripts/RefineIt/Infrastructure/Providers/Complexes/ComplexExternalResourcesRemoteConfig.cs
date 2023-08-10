using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ExternalResources", menuName = "RemoteConfigs/Complex/ExternalResources", order = 0)]
    public class ComplexExternalResourcesRemoteConfig : AbstractConfigProvider
    {
        [PageName("ComplexExternalResources")] public List<ComplexRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}