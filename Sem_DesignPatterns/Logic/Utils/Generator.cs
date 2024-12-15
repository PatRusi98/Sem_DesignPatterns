using Sem_DesignPatterns.Logic.Objects;
using static Sem_DesignPatterns.Logic.Utils.Enums;

namespace Sem_DesignPatterns.Logic.Utils
{
    public class Generator
    {
        private static Generator? _instance = null;
        private readonly Random _random = new();

        private Generator() { }

        public static Generator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new();

                return _instance;
            }
        }

        public GeoEntity GenerateEntity(GeoEntityType type = GeoEntityType.Unknown, List<GPSLocation[]>? listGps = null)
        {
            var property = type switch
            {
                GeoEntityType.Parcel => false,
                GeoEntityType.Property => true,
                _ => _random.Next() % 2 == 0,
            };
            var number = _random.Next();
            GPSLocation[]? gpss = [];

            if (listGps != null)
            {
                var count = listGps.Count;
                if (count > 0)
                {
                    gpss = listGps[_random.Next(count)];

                    if (property)
                        listGps.Remove(gpss);

                }
                else
                {
                    gpss = GenerateGPSLocations();
                }
            }
            else
            {
                gpss = GenerateGPSLocations();
            }

            var description = "Description of " + (property ? "property" : "parcel") + " number: " + number;
            GeoEntity result;

            if (property)
            {
                result = new Property(number, description, gpss[0], gpss[1], GeoEntityType.Property);
            }
            else
            {
                result = new Parcel(number, description, gpss[0], gpss[1], GeoEntityType.Parcel);
            }

            return result;
        }

        public List<GPSLocation[]> GenerateListOfGPSLocations(int number)
        {
            List<GPSLocation[]> result = new();

            for (int i = 0; i < number; i++)
            {
                result.Add(GenerateGPSLocations());
            }

            return result;
        }

        private GPSLocation[] GenerateGPSLocations()
        {
            var lat1 = Math.Round(_random.NextDouble() * 89, 4);                                            // generovanie nahodnych GPS lokalit, maximalne do 89 sirky a 179 dlzky aby sa neprelievali
            var longt1 = Math.Round(_random.NextDouble() * 179, 4);
            var lat2 = Math.Round(_random.NextDouble(), 4) + lat1;                                          // logika, aby negenerovalo zbytocne velke objekty +- max 1.0 sirky a dlzky
            var longt2 = Math.Round(_random.NextDouble(), 4) + longt1;
            Coordinate latCoord = _random.Next(2) == 0 ? Coordinate.North : Coordinate.South;               // nahodny vyber kvadrantu
            Coordinate longCoord = _random.Next(2) == 0 ? Coordinate.East : Coordinate.West;

            GPSLocation[] result =
            [
                new GPSLocation()
                {
                    Latitude = lat1,
                    LatCoord = latCoord,
                    Longitude = longt1,
                    LongCoord = longCoord
                },
                new GPSLocation()
                {
                    Latitude = lat2,
                    LatCoord = latCoord,
                    Longitude = longt2,
                    LongCoord = longCoord
                }
            ];

            return result;
        }
    }
}
