using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

namespace DataLoadAnalyzer.Data
{
    public class DateToLongConverter : DefaultTypeConverter
    {   
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var dateValue = (DateTime)value;

            return dateValue.Ticks.ToString();
        }        
    }

}
