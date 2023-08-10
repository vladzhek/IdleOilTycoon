using System;

namespace Configs
{
    [Serializable]
    public class ProcessingProduceTwoResourcesRemoteData
    {
        public int RequiredResource1;
        public int RequiredResource2;
        public int ProduceResource;
        public int StorageProduceResource;
        public int StorageRequiredResource1;
        public int StorageRequiredResource2;
        public int LevelPrice;
        public int ProcessingTime;
    }
}