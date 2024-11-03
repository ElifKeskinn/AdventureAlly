using System.Text.Json.Serialization;

namespace CleanArchitecture.Core.Models
{
    public class GeocodeResponse
    {
        [JsonPropertyName("results")]
        public GeocodeResult[] Results { get; set; }
    }

    public class GeocodeResult
    {
        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        [JsonPropertyName("location")]
        public Coordinate Location { get; set; }
    }
}
