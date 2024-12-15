using Sem_DesignPatterns.Logic.Objects;
using System.ComponentModel;
using System.Reflection;
using static Sem_DesignPatterns.Logic.Utils.Enums;

namespace Sem_DesignPatterns.Logic.Utils
{
    public static class Extensions
    {
        public static char CoordinateToChar(this Coordinate coordinate)
        {
            return (char)coordinate;
        }

        public static Coordinate CharToCoordinate(this char c)
        {
            return c switch
            {
                'E' => Coordinate.East,
                'N' => Coordinate.North,
                'S' => Coordinate.South,
                'W' => Coordinate.West,
                _ => Coordinate.Unknown,
            };
        }

        public static bool IsLatitude(this Coordinate coordinate)
        {
            return coordinate == Coordinate.North || coordinate == Coordinate.South;
        }

        public static bool IsLongitude(this Coordinate coordinate)
        {
            return coordinate == Coordinate.East || coordinate == Coordinate.West;
        }

        public static double[] GPSToDouble(this GPSLocation gps)
        {
            var latitude = GPSDoublePosition(gps.Latitude, gps.LatCoord);
            var longitude = GPSDoublePosition(gps.Longitude, gps.LongCoord);

            return [latitude, longitude];
        }

        public static int CompareKeys(this object valueKey, object currNodeKey)
        {
            if (valueKey is IComparable a && currNodeKey is IComparable b)
            {
                return a.CompareTo(b);
            }

            throw new ArgumentException("Key is not comparable!");
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString())!;
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>()!;

            return attribute != null ? attribute.Description : value.ToString();
        }

        #region private
        private static double GPSDoublePosition(double value, Coordinate coord)
        {
            return coord switch                                                             // prehodenie gps pozicii do double - sirka od 0.0 do 180.0 a dlzka od 0.0 do 360.0
            {                                                                               // kvoli klucom - jednoduchsie ako porovnavat v strome GPSPosition
                Coordinate.North => 90.00 + value,
                Coordinate.East => 180.00 + value,
                Coordinate.South => 90.00 - value,
                Coordinate.West => 180.00 - value,
                _ => Double.MinValue,
            };
        }

        #endregion
    }
}
