namespace IndicesCollectionReader.Entity
{
    [Serializable]
    public class Indexes // объектная модель индексов
    {
        public NormativeTable BasicLevelNormativeTable = new NormativeTable();
        public NormativeTable CurrentLevelNormativeTable = new NormativeTable();
    }
}
