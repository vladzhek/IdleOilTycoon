using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Coke", menuName = "RemoteConfigs/Complex/Coke", order = 0)]
    public class ComplexCokeRemoteConfig : AbstractConfigProvider
    {
        [PageName("ComplexCoke")] public List<ComplexRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}