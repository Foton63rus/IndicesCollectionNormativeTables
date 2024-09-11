using IndicesCollectionReader_NativeTables.Entity;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;

namespace IndicesCollectionReader.Entity
{
    public  class ContentModelBuilder
    {
        PdfDocument pdf;
        ContentExtractor content;
        Indexes indexes = new Indexes();   // Объектная модель файла
        string chapterName = "";
        (int, int) chapterCashData = (0, 0);
        ChapterWithCompilations currentChapterWithCompilations = null;
        ChapterWithSections currentChapterWithSections = null;
        Compilation currentCompilation = null;
        Section currentSection = null;
        Table currentTable = null;
        string currentTableHeader = "";
        NormativeTable currentNormativeTable = null;
        List<ISubTableParser> subTableParsers = new List<ISubTableParser>();

        int firstIndex = 0;
        int firstPage = 0;
        int lastPage = 0;

        public ContentModelBuilder(PdfDocument pdfDocument, ContentExtractor contentExtractor)
        {
            subTableParsers.Add(new SubTable9ColumnsParser());
            subTableParsers.Add(new SubTable7ColumnsParser());
            Console.Clear();
            Console.WriteLine("Содержание извлечено. Идет построение структуры файла. Немного подождите!");
            this.pdf = pdfDocument;
            this.content = contentExtractor;
            buildContentModel();
        }

        private void buildContentModel()
        {
            currentChapterWithCompilations = null;
            currentChapterWithSections = null;
            currentCompilation = null;
            currentSection = null;
            currentTable = null;

            for (int i = 0; i < content.contentData.Count; i++)
            {
                ContentData context = content.contentData[i];
                buildNormativeTable(context, i);
            }
        }
        private void buildChapter(ContentData context)
        {
            int number = 0;
            Regex regex = new Regex(RegexTemplates.ChapterNumber);
            Match match = regex.Match(context.Name);
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, out number);
                chapterName = context.Name;
            }
            int page = context.Page;
            chapterCashData = (number, page);

            currentChapterWithCompilations = null;
            currentChapterWithSections = null;
            currentCompilation = null;
            currentSection = null;
            currentTable = null;
        }
        private void buildSection(ContentData context)
        {
            if (chapterCashData != (0, 0))
            {
                currentChapterWithCompilations = null;
                currentChapterWithSections = new ChapterWithSections(chapterCashData.Item1, chapterCashData.Item2);
                currentChapterWithSections.Name = chapterName;
                ///indexes.Chapters.Add(currentChapterWithSections);
                chapterCashData = (0, 0);
            }
            currentSection = new Section();
            currentTable = null;
            Regex regex = new Regex(RegexTemplates.SectionNumber);
            Match match = regex.Match(context.Name);
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, out currentSection.Number);
                currentSection.Name = match.Value;
            }
            currentSection.Page = context.Page;
            currentChapterWithSections.Sections.Add(currentSection);
        }
        private void buildNormativeTable(ContentData context, int index)
        {

            ///основная логика парса конкретной нормативной таблицы
            (int, int, int) borders = defineLevelTypeAndBordersForNormativeTable(context, index);
            
            for (int currentPage = borders.Item1; currentPage <= borders.Item3; currentPage++) 
            {//постраничный проход
                string currentPageTextFull = pdf.GetPage(currentPage).Text;
                int firstCurrentIndex = 0;
                if (currentPage == borders.Item1) firstCurrentIndex = borders.Item2;

                string currentPageText = currentPageTextFull.Substring(firstCurrentIndex, currentPageTextFull.Length - firstCurrentIndex).Trim();
                //Console.WriteLine(currentPageText);
                if (currentPage == 370)
                {
                    Console.WriteLine(currentPageText);
                }
                Dictionary<int, ISubTableParser> subTableHeaders = new Dictionary<int, ISubTableParser>();

                foreach (ISubTableParser subTableParser in subTableParsers)
                {
                    Regex regexHeader = new Regex(subTableParser.GetHeaderTamplate());
                    MatchCollection headerMatches = regexHeader.Matches(currentPageText);
                    foreach (Match match in headerMatches)
                    {
                        subTableHeaders.Add(match.Index, subTableParser);
                        //Console.WriteLine($"{match.Index} - {match.Index + match.Length}");
                    }
                }
                try
                {
                    for (int i = 0; i < subTableHeaders.Count; i++)
                    {
                        var currentKVSubTableParser = subTableHeaders.ElementAt(i);
                        int firstSTIndex = currentKVSubTableParser.Key + currentKVSubTableParser.Value.GetHeaderTamplate().Length;
                        int lastSTIndex = currentPageText.Length;

                        if (i == subTableHeaders.Count - 1)
                        {

                        }
                        else
                        {
                            lastSTIndex = subTableHeaders.ElementAt(i + 1).Key - 1;
                        }
                        
                        string subtableText = currentPageText.Substring(firstSTIndex, lastSTIndex - firstSTIndex);

                        List<IRecord> records = currentKVSubTableParser.Value.GetRecords(subtableText);

                        NT_SubTable newST = new NT_SubTable();
                        newST.SubTableDescription = currentKVSubTableParser.Value.GetDescription();
                        newST.records = records;
                        var subNT = currentNormativeTable.SubTables.Any(x => x.SubTableDescription == currentKVSubTableParser.Value.GetDescription());
                        if (subNT)
                        {
                            currentNormativeTable.SubTables.First(x => x.SubTableDescription == currentKVSubTableParser.Value.GetDescription()).records.AddRange(records);
                        }
                        else
                        {
                            currentNormativeTable.SubTables.Add(newST);
                        }

                    }
                }catch(Exception ex)
                {
                    Console.WriteLine("//////////////////////");
                    Console.WriteLine($"currentPage: {currentPage}");
                    Console.WriteLine($"currentPage: {currentPage}");
                    Console.WriteLine($"currentPage: {currentPage}");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(ex.StackTrace);
                }
                
            }
        }
        private (int, int, int) defineLevelTypeAndBordersForNormativeTable(ContentData context, int index)
        {
            firstPage = context.Page;
            firstIndex = context.Index + context.Name.Length;

            if (index == 0)
            {
                lastPage = content.contentData[1].Page - 1;
                if (lastPage < firstPage) throw new IndexOutOfRangeException("не должно так быть =)");
            }
            else //(index == 1)
            {
                lastPage = pdf.NumberOfPages;
            }

            var a = 1;

            if (context.Name.ToLower().Contains("работ в зимнее время в базисном уровне"))
            {
                Console.WriteLine("в базисном уровне");
                currentNormativeTable = indexes.BasicLevelNormativeTable;
                //lastPage = 
            }
            else if (context.Name.ToLower().Contains("работ в зимнее время в текущем уровне"))
            {
                Console.WriteLine("в текущем уровне");
                currentNormativeTable = indexes.CurrentLevelNormativeTable;
            }
            ///(int firstPage, int firstIndex, int lastPage) borders
            return (firstPage, firstIndex, lastPage);
        }
        public string GetJSON()
        {
            return JsonConvert.SerializeObject(indexes, Formatting.Indented);
        }

        public Indexes GetModel()
        {
            return indexes;
        }
    }
}
