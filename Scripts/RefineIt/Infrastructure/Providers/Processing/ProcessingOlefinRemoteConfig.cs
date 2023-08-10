using System.Collections.Generic;
using System.Linq;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Olefin", menuName = "RemoteConfigs/Processing/Olefin", order = 0)]
    public class ProcessingOlefinRemoteConfig : AbstractConfigProvider
    {
        [PageName("ProcessingOlefin")] public List<ProcessingSingleResourceRemoteData> Data;

        public override List<IConfigData> GetData()
        {
            return Data.Cast<IConfigData>().ToList();
        }
    }
}