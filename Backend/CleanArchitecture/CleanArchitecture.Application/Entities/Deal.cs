using System;
//Turistlere sunulan indirimler ve teklifleri temsil eder. Turistlerin alabileceði fýrsatlarý tanýmlar.
namespace CleanArchitecture.Core.Entities
{
    public class Deal
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime ValidStartDate { get; set; }
        public DateTime ValidEndDate { get; set; }
        public string Place { get; set; }
    }
}
