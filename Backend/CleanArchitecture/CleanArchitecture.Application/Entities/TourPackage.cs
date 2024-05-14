using System.Collections.Generic;
// Turistlere sunulan tur paketlerini temsil eder. Turistlere özel gezi veya etkinlikleri paketler halinde sunar.
namespace CleanArchitecture.Core.Entities
{
    public class TourPackage
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Destinations { get; set; }
        public List<string> Activities { get; set; }
        public decimal Price { get; set; }
    }
}
