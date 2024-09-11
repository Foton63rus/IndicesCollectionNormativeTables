using IndicesCollectionReader.Entity;
using UglyToad.PdfPig;

namespace IndicesCollectionReader
{
    public class PdfFile
    {
        //ведется импорт нормативных таблиц
        PdfDocument pdf;
        ContentExtractor content;
        ContentModelBuilder contentModelBuilder;

        Indexes indexes = null;   // Объектная модель файла

        public string Open(string path)
        {
            ClearPdfData();
            try
            {
                pdf = PdfDocument.Open(path);
                Console.WriteLine(pdf.GetPage(370).Text);
                return ExtractPdfData(pdf);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string ExtractPdfData(PdfDocument pdf)
        {

            content = new ContentExtractor(pdf);   //Извлечение данных из содержания файла
            contentModelBuilder = new ContentModelBuilder(pdf, content);
            indexes = contentModelBuilder.GetModel();

            return contentModelBuilder.GetJSON();
        }
        
        private void ClearPdfData()
        {
            indexes = new Indexes();
            if (pdf != null) pdf.Dispose();
        }
    }
}
