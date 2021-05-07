using CsvHelper.Configuration.Attributes;
using System;

namespace DataLoadAnalyzer.Models
{
    public class ProductSaleHistoryInput
    {
        [Index(0)]
        public long SkuId { get; set; }

        [Index(1)]
        public long ProductId { get; set; }
       

        [Index(3)]
        public string ProductName { get; set; }

        [Index(4)]
        public long VariantId { get; set; }

        [Index(5)]
        public string VariantName { get; set; }

        [Index(6)]
        public long ConditionId { get; set; }

        [Index(7)]
        public string ConditionName { get; set; }

        [Index(8)]
        public long LanguageId { get; set; }

        [Index(9)]
        public string LanguageName { get; set; }

        [Index(10)]
        public int QuantitySold { get; set; }

        [Index(11)]
        public DateTime OrderDate { get; set; }

        [Index(12)]
        public decimal PurchasePrice { get; set; }

        [Index(13)]
        public decimal ShippingPrice { get; set; }
    }
}
