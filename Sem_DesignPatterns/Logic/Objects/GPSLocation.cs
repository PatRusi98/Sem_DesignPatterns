using static Sem_DesignPatterns.Logic.Utils.Enums;

namespace Sem_DesignPatterns.Logic.Objects
{
    public struct GPSLocation
    {
        public required double Latitude { get; set; }
        public required Coordinate LatCoord { get; set; }
        public required double Longitude { get; set; }
        public required Coordinate LongCoord { get; set; }

        public override readonly string ToString() => $"{Latitude}~{LatCoord}~{Longitude}~{LongCoord}";
    }
}
