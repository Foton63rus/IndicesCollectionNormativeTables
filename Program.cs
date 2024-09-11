using IndicesCollectionReader;

PdfFile pdfFile;

while (true)
{
    Console.WriteLine("Укажи путь к файлу с индексами");
    string input = Console.ReadLine();

    switch (input)
    {
        default:
            pdfFile = new PdfFile();
            string JSON = pdfFile.Open(input);
            //Console.Clear();
            Console.WriteLine("writing json");
            string outputName = $"{input}_normativeTables.json";
            File.WriteAllText(outputName, JSON);
            //Console.Clear();
            Console.WriteLine($"Файл успешно записан: {outputName}");
            break;
    }
}