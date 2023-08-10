using System;

namespace Configs
{
    [Serializable]
    public class NaftaProcessingConfigData : IConfigData
    {
        public string InternalId { get; }

        public int RequiredOil;
        public int ProduceNafta;
        public int ProduceVg;
        public int ProduceDtvs;
        public int ProduceGoodron;
        public int LevelPrice;
        public int StorageNafta;
        public int StorageVg;
        public int StorageDtvs;
        public int StorageGoodron;
        public int StorageOil;
    }
}