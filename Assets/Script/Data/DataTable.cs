using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public abstract class DataTable
{
    public static readonly string FormatPath = "DataTables/{0}";

    public abstract void Load(string filename);

    public static List<T> LoadCSV<T>(string csvText)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            
            Delimiter = "\t", // ¡ç ÅÇ ±¸ºÐÀÚ
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            PrepareHeaderForMatch = a =>
            {
                var h = a.Header ?? string.Empty;
                return h.Replace("\"", "")
                        .Replace("\r", "")
                        .Replace("\n", "")
                        .Trim();
            },
        };

        using (var reader = new StringReader(csvText))
        //using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        using (var csvReader = new CsvReader(reader, config))
        {
            var records = csvReader.GetRecords<T>();
            return records.ToList();
        }
    }
}