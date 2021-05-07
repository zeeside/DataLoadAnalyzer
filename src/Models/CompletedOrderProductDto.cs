using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoadAnalyzer.Models
{
    public class CompletedOrderProductDto
    {
        [Index(0)]
        public long SellerOrderId { get; set; }
        
        [Index(1)]
        public long ProductId { get; set; }
        
        [Index(2)]
        public long StoreProductConditionId { get; set; }

        [Index(3)]
        public string ProductName { get; set; }

        [Index(4)]
        public long ConditionId { get; set; }

        [Index(5)]
        public string ConditionName { get; set; }

        [Index(6)]
        public long SuperConditioinId { get; set; }

        [Index(7)]
        public string SuperConditionName { get; set; }

        [Index(8)]
        public string LanguageId { get; set; }

        [Index(9)]
        public string LanguageName { get; set; }

        [Index(10)]
        public int Quantity { get; set; }

        [Index(11)]
        public long CreatedAt { get; set; }

        [Index(12)]
        public decimal Price { get; set; }

        [Index(13)]
        public decimal  ShippingAmount { get; set; }
        
    }
}
