namespace HW10.Models
{
    public class Image
    {
		public int Id { get; set; }
		public string Filename { get; set; } = null!;

        public string Src => "/uploads/" + Filename;
    }
}
