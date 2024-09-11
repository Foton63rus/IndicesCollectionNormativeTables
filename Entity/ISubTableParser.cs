using IndicesCollectionReader.Entity;

namespace IndicesCollectionReader_NativeTables.Entity
{
    internal interface ISubTableParser
    {
        public string GetHeaderTamplate();
        public string GetDescription();
        public List<IRecord> GetRecords(string subTableText);
    }
}
