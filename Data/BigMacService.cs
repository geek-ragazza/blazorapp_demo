using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion; 
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BlazorApp.Data
{
    public class BigMacDataService
    {
        private readonly string _dataFilePath;

        public BigMacDataService(IWebHostEnvironment webHostEnvironment)
        {
            _dataFilePath = Path.Combine(webHostEnvironment.WebRootPath, "csv", "big-mac-source-data-v2.csv");
        }

        public List<BigMacData> GetBigMacData()
        {
            try
            {
                using (var reader = new StreamReader(_dataFilePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound = null // Handle missing fields by setting them to null
                }))
                {
                    csv.Context.RegisterClassMap<BigMacDataMap>(); // Register the custom class map
                    return csv.GetRecords<BigMacData>()
                        .Where(data =>
                            IsValidDataField(data.LocalPrice) &&
                            IsValidDataField(data.DollarExchangeRate) &&
                            IsValidDataField(data.GdpDollar))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log or throw)
                // Logging: log.Error("Error reading CSV file", ex);
                // Rethrow the exception if necessary
                throw;
            }
        }

        private bool IsValidDataField(decimal? fieldValue)
        {
            return fieldValue.HasValue && fieldValue >= 0; // Change this condition as needed
        }
    }

    public class BigMacDataMap : ClassMap<BigMacData>
    {
        public BigMacDataMap()
        {
            References<CountryMap>(m => m.Country);
            Map(m => m.CurrencyCode).Name("currency_code");
            Map(m => m.LocalPrice).Name("local_price").TypeConverter<CustomDecimalConverter>();
            Map(m => m.DollarExchangeRate).Name("dollar_ex");
            Map(m => m.GdpDollar).Name("GDP_dollar").TypeConverter<EmptyStringToZeroDecimalConverter>();
            Map(m => m.GdpLocal).Name("GDP_local").TypeConverter<EmptyStringToZeroDecimalConverter>();
            Map(m => m.Date).Name("date");
        }
    }


    public class CountryMap : ClassMap<Country>
    {
        public CountryMap()
        {
            Map(m => m.Name).Name("name");
            Map(m => m.IsoA3).Name("iso_a3");
        }
    }
}
public class EmptyStringToZeroDecimalConverter : DecimalConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            // Convert empty strings to zero for decimal fields
            return 0m;
        }

        return base.ConvertFromString(text, row, memberMapData);
    }
}

public class CustomDecimalConverter : DecimalConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return base.ConvertFromString("0", row, memberMapData); // Convert empty strings to zero
        }

        if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        return base.ConvertFromString(text, row, memberMapData);
    }
}
