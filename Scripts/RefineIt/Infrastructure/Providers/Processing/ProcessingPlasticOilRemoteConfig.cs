using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Plastic", menuName = "RemoteConfigs/Processing/Plastic", order = 0)]
    public class ProcessingPlasticOilRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingPlastic")] public List<ProcessingSingleResourceRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}