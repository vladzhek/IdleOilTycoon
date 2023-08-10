using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Bitum", menuName = "RemoteConfigs/Processing/Bitum", order = 0)]
    public class ProcessingBitumRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingBitum")] public List<ProcessingSingleResourceRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}