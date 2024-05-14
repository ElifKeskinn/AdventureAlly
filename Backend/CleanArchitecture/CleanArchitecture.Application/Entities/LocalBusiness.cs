using System.Collections.Generic;
// Turistlere indirim sunan veya özel anlaþmalarý olan yerel iþletmeleri temsil eder.
namespace CleanArchitecture.Core.Entities
{
    public class LocalBusiness
    {
        public string Name { get; set; }
        public string Place { get; set; }
        public string Category { get; set; }
        public List<Deal> Discounts { get; set; }
    }
}
