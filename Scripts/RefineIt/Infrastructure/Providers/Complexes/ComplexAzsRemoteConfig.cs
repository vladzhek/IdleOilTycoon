using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Azs", menuName = "RemoteConfigs/Complex/Azs", order = 0)]
    public class ComplexAzsRemoteConfig : AbstractConfigProvider
    {
        [PageName("ComplexAzs")] public List<ComplexRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}