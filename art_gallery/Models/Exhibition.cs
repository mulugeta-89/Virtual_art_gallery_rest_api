namespace art_gallery.Models
{
    public class Exhibition
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Art> Artworks { get; set; }
    }
}
