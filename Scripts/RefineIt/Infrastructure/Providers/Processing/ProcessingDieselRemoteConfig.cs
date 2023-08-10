using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Diesel", menuName = "RemoteConfigs/Processing/Diesel", order = 0)]
    public class ProcessingDieselRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingDiesel")] public List<ProcessingProduceTwoResourcesRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}