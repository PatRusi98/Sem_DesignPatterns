using System.ComponentModel;

namespace Sem_DesignPatterns.UI.Models
{
    public class GeoEntityModel : BaseModel
    {
        [Description("Typ")]
        public string Type { get; set; }

        [Description("Nambr")]
        public int Number { get; set; }

        [Description("Deskripsn")]
        public string Description { get; set; }

        [Description("GPS LL")]
        public string GPS1 { get; set; }

        [Description("GPS UR")]
        public string GPS2 { get; set; }
    }
}
