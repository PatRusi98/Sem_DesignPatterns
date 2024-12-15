using static Sem_DesignPatterns.Logic.Utils.Enums;

namespace Sem_DesignPatterns.Logic.Objects
{
    public class Parcel(int number, string description, GPSLocation point1, GPSLocation point2, GeoEntityType type) : GeoEntity(number, description, point1, point2, type) { }
}
