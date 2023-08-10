using System;

namespace Configs
{
    [Serializable]
    public class ProcessingSingleResourceRemoteData
    {
        public int RequiredResource;
        public int ProduceResource;
        public int StorageProduceResource;
        public int StorageRequiredResource;
        public int LevelPrice;
        public int ProcessingTime;
    }
}