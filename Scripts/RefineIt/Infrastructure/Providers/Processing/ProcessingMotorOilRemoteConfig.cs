using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "MotorOil", menuName = "RemoteConfigs/Processing/MotorOil", order = 0)]
    public class ProcessingMotorOilRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingMotorOil")] public List<ProcessingProduceTwoResourcesRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}