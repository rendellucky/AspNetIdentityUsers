namespace HW10.Models
{
    public class MapMarker
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}
