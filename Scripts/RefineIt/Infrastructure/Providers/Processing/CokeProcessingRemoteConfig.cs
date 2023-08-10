using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Coke", menuName = "RemoteConfigs/Processing/Coke", order = 0)]
    public class CokeProcessingRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingCoke")] public List<ProcessingSingleResourceRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}