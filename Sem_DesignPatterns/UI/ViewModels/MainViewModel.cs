using Sem_DesignPatterns.UI.Models;
using System.Collections.ObjectModel;

namespace Sem_DesignPatterns.UI.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<GeoEntityModel> Items { get; set; }

        public MainViewModel()
        {
            Items = new ObservableCollection<GeoEntityModel>
            {
                new GeoEntityModel { Type = "Parcel", Number = 1234, Description = "adad", GPS1 = "48.123N, 17.456E", GPS2 = "48.124N, 17.457E" },
                new GeoEntityModel { Type = "Parcel", Number = 5678, Description = "adad", GPS1 = "48.223N, 17.556E", GPS2 = "48.224N, 17.557E" },
                new GeoEntityModel { Type = "Parcel", Number = 9101, Description = "adad", GPS1 = "48.323N, 17.656E", GPS2 = "48.324N, 17.657E" }
            };
        }
    }
}
