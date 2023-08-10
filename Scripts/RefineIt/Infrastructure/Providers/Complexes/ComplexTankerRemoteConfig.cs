using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Tanker", menuName = "RemoteConfigs/Complex/Tanker", order = 0)]
    public class ComplexTankerRemoteConfig : AbstractConfigProvider
    {
        [PageName("ComplexTanker")] public List<ComplexRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}