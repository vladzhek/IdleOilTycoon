using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Ethanol", menuName = "RemoteConfigs/Processing/Ethanol", order = 0)]
    public class ProcessingEthanolRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingEthanol")] public List<ProcessingSingleResourceRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}