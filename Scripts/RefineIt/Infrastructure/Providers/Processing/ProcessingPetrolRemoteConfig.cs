using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Petrol", menuName = "RemoteConfigs/Processing/Petrol", order = 0)]
    public class ProcessingPetrolRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingPetrol")] public List<ProcessingSingleResourceRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}