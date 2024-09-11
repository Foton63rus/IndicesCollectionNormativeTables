using IndicesCollectionReader.Entity;

namespace IndicesCollectionReader_NativeTables.Entity
{
    [Serializable]
    public class NT_SubTable
    {
        public string SubTableDescription;
        
        public List<IRecord> records = new List<IRecord>();
    }
}
