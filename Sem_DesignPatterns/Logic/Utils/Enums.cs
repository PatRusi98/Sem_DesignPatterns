using System.ComponentModel;

namespace Sem_DesignPatterns.Logic.Utils
{
    public static class Enums
    {
        public enum Coordinate : short
        {
            [Description("Unknown")]
            Unknown = -1,
            [Description("East")]
            East = 69,                                      // ASCII kod pre 'E'
            [Description("North")]
            North = 78,                                     // ASCII kod pre 'N'
            [Description("South")]
            South = 83,                                     // ASCII kod pre 'S'
            [Description("West")]
            West = 87                                       // ASCII kod pre 'W'
        }

        public enum GeoEntityType : short
        {
            [Description("Unknown")]
            Unknown = -1,
            [Description("Parcel")]
            Parcel = 0,
            [Description("Property")]
            Property = 1
        }
    }
}
