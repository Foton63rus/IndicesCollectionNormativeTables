using IndicesCollectionReader.Entity;
using System.Text.RegularExpressions;

namespace IndicesCollectionReader_NativeTables.Entity
{
    internal class SubTable9ColumnsParser : ISubTableParser
    {
        /// <summary>
        /// Это парсер строк для подтаблицы с 9-ю колонками
        /// 1 - Наименование сборников расценок, номера таблиц и расценок
        /// 2 - СМР - нормы в % - НР
        /// 3 - СМР - нормы в % - СП
        /// 4 - СМР - Коэффициенты, учитывающие дополнительные затраты, связанные с производством работ в зимнее время - к затратам труда, заработной плате рабочих и стоимости эксплуатации машин
        /// 5 - СМР - Коэффициенты, учитывающие дополнительные затраты, связанные с производством работ в зимнее время - к стоимости материалов
        /// 6 - Другие главы - нормы в % - НР
        /// 7 - Другие главы - нормы в % - СП
        /// 8 - Другие главы - Коэффициенты, учитывающие дополнительные затраты, связанные с производством работ в зимнее время - к затратам труда, заработной плате рабочих и стоимости эксплуатации машин
        /// 9 - Другие главы - Коэффициенты, учитывающие дополнительные затраты, связанные с производством работ в зимнее время - к стоимости материалов
        /// </summary>

        private const string headerTMP = @"Наименование\s+сборников\s+расценок,\s+номера\s+таблиц\s+и\s+расценок\s+Строительно-монтажные\s+работы\s+При\s+использовании\s+расценок\s+других\s+глав\s+при\s+определении\s+стоимости ремонтно-строительных\s+работ\s+Нормы\s+в\s+%\s+Коэффициенты,\s+учитывающие\s+дополнительные\s+затраты,\s+связанные\s+с\s+производством\s+работ\s+в\s+зимнее\s+время\s+Нормы\s+в\s+%\s+Коэффициенты,\s+учитывающие\s+дополнительные\s+затраты,\s+связанные\s+с\s+производством\s+работ\s+в\s+зимнее\s+время\s+НР\s+СП\s+к\s+затратам\s+труда,\s+заработной\s+плате\s+рабочих\s+и\s+стоимости\s+эксплуатации\s+машин\s+к\s+стоимости\s+материалов\s+НР\s+СП\s+к\s+затратам\s+труда,\s+заработной\s+плате\s+рабочих\s+и\s+стоимости\s+эксплуатации\s+машин\s+к\s+стоимости\s+материалов\s+1\s+2\s+3\s+4\s+5\s+6\s+7\s+8\s+9\s+";
        private const string strokeTMP = @"(\d+.\d+-\d+-\d+)\s+(\d+.\d+-\d+-\d+\s+)?(\d+)\s+(\d+)\s+(\d+,\d+)\s+(\d+,\d+)\s+(\d+)\s+(\d+)\s+(\d+,\d+)\s+(\d+,\d+)\s?";
        
        private const string description = $"1 - Наименование сборников расценок, номера таблиц и расценок\r\n        /// 2 - СМР - нормы в % - НР\r\n        /// 3 - СМР - нормы в % - СП\r\n        /// 4 - СМР - Коэффициенты, учитывающие дополнительные затраты, связанные с производством работ в зимнее время - к затратам труда, заработной плате рабочих и стоимости эксплуатации машин\r\n        /// 5 - СМР - Коэффициенты, учитывающие дополнительные затраты, связанные с производством работ в зимнее время - к стоимости материалов\r\n        /// 6 - Другие главы - нормы в % - НР\r\n        /// 7 - Другие главы - нормы в % - СП\r\n        /// 8 - Другие главы - Коэффициенты, учитывающие дополнительные затраты, связанные с производством работ в зимнее время - к затратам труда, заработной плате рабочих и стоимости эксплуатации машин\r\n        /// 9 - Другие главы - Коэффициенты, учитывающие дополнительные затраты, связанные с производством работ в зимнее время - к стоимости материалов";

        private Regex regex = new Regex(strokeTMP);
        string ISubTableParser.GetHeaderTamplate() => headerTMP;
        string ISubTableParser.GetDescription() => description; 
        List<IRecord> ISubTableParser.GetRecords(string subTableText)
        {
            MatchCollection matchCollection = regex.Matches(subTableText);
            List<IRecord> records = new List<IRecord>();
            foreach (Match match in matchCollection)
            {
                records.Add(new Record9ColumnsTable( 
                    match.Groups[1].Value,
                    match.Groups[2].Value.Trim(),
                    match.Groups[3].Value,
                    match.Groups[4].Value,
                    match.Groups[5].Value,
                    match.Groups[6].Value,
                    match.Groups[7].Value,
                    match.Groups[8].Value,
                    match.Groups[9].Value,
                    match.Groups[10].Value
                    ));
            }
            return records;
        }
    }

    [Serializable]
    public class Record9ColumnsTable : IRecord
    {
        public string col1a;
        public string col1b;

        public string col2;
        public string col3;
        public string col4;
        public string col5;
        public string col6;
        public string col7;
        public string col8;
        public string col9;
        public Record9ColumnsTable( string col1a, string col1b, string col2, string col3, string col4, string col5, string col6, string col7, string col8, string col9 )
        {
            this.col1a = col1a;
            this.col1b = col1b;
            this.col2 = col2;
            this.col3 = col3;
            this.col4 = col4;
            this.col5 = col5;
            this.col6 = col6;
            this.col7 = col7;
            this.col8 = col8;
            this.col9 = col9;
        }
    }
}
