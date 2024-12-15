using Sem_DesignPatterns.Logic.Objects;

namespace Sem_DesignPatterns.Logic.Utils
{
    public struct GeoEntityParams
    {
        public int? Number { get; set; } = null;
        public string? Description { get; set; } = null;
        public GPSLocation? Point1 { get; set; } = null;
        public GPSLocation? Point2 { get; set; } = null;

        public GeoEntityParams() { }
    }
}
