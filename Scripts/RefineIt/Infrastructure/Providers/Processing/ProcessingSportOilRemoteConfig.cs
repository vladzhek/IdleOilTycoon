using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "SportOil", menuName = "RemoteConfigs/Processing/SportOil", order = 0)]
    public class ProcessingSportOilRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingSportOil")] public List<ProcessingProduceTwoResourcesRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}