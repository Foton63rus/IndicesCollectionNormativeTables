using IndicesCollectionReader.Entity;
using System.Text.RegularExpressions;

namespace IndicesCollectionReader_NativeTables.Entity
{
    internal class SubTable7ColumnsParser : ISubTableParser
    {
        /// <summary>
        /// Это парсер строк для подтаблицы с 7-ю колонками
        /// 1 - № п/п
        /// 2 - Номера таблиц, шифры НЦКР
        /// 3 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Затратам труда,заработной плате рабочих для НЦКР
        /// 4 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Стоимости эксплуатации машин для НЦКР
        /// 5 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Стоимости материалов для НЦКР
        /// 6 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Накладным расходам для НЦКР
        /// 7 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Сметной прибыли для НЦКР
        /// </summary>

        private const string headerTMP = @"№\s?п\/п\s+Номера\s+таблиц,\s+шифры\s+НЦКР\s+Коэффициенты,\s+учитывающие дополнительные\s+затраты,\s+связанные\s+с\s+производством\s+работ\s+в\s+зимнее\s+время\s+к\s+Затратам\s+труда,\s+заработной\s+плате\s+рабочих\s+для\s+НЦКР\s+Стоимости\s+эксплуатации\s+машин\s+для\s+НЦКР\s+Стоимости\s+материалов\s+для\s+НЦКР\s+Накладным\s+расходам\s+для\s+НЦКР\s+Сметной\s+прибыли\s+для\s+НЦКР\s+1\s+2\s+3\s+4\s+5\s+6\s+7";
        private const string strokeTMP = @"(\d+\s+)(\d+.\d+-\d+-\d+)\s+(\d+.\d+-\d+-\d+\s+)?(\d+,\d+\s+)(\d+,\d+\s+)(\d+,\d+\s+)(\d+,\d+\s+)(\d+,\d+\s?)";
        
        private const string description = $"1 - № п/п\r\n        /// 2 - Номера таблиц, шифры НЦКР\r\n        /// 3 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Затратам труда,заработной плате рабочих для НЦКР\r\n        /// 4 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Стоимости эксплуатации машин для НЦКР\r\n        /// 5 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Стоимости материалов для НЦКР\r\n        /// 6 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Накладным расходам для НЦКР\r\n        /// 7 - Коэф-ы, учитывающие доп. затраты, связанные с пр-вом работ в зимнее время к Сметной прибыли для НЦКР";

        private Regex regex = new Regex(strokeTMP);
        string ISubTableParser.GetHeaderTamplate() => headerTMP;
        string ISubTableParser.GetDescription() => description; 
        List<IRecord> ISubTableParser.GetRecords(string subTableText)
        {
            MatchCollection matchCollection = regex.Matches(subTableText);
            List<IRecord> records = new List<IRecord>();
            foreach (Match match in matchCollection)
            {
                records.Add(new Record7ColumnsTable( 
                    match.Groups[1].Value,
                    match.Groups[2].Value,
                    match.Groups[3].Value.Trim(),
                    match.Groups[4].Value,
                    match.Groups[5].Value,
                    match.Groups[6].Value,
                    match.Groups[7].Value,
                    match.Groups[8].Value
                    ));
            }
            return records;
        }
    }

    [Serializable]
    public class Record7ColumnsTable : IRecord
    {
        public string col1;
        public string col2a;
        public string col2b;
        public string col3;
        public string col4;
        public string col5;
        public string col6;
        public string col7;
        public Record7ColumnsTable( string col1, string col2a, string col2b, string col3, string col4, string col5, string col6, string col7 )
        {
            this.col1 = col1;
            this.col2a = col2a;
            this.col2b = col2b;
            this.col3 = col3;
            this.col4 = col4;
            this.col5 = col5;
            this.col6 = col6;
            this.col7 = col7;
        }
    }
}
