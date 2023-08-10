using System.Collections.Generic;
using NorskaLib.GoogleSheetsDatabase;
using UnityEngine;

namespace Configs
{
    public abstract class AbstractConfigProvider : DataContainerBase, IConfigProvider
    {
        public abstract List<IConfigData> GetData();
    }
}