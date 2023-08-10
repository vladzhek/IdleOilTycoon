using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Nafta", menuName = "RemoteConfigs/Processing/Nafta", order = 0)]
    public class NaftaProcessingRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingNafta")] public List<NaftaProcessingConfigData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}