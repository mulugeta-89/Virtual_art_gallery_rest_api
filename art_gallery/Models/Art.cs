namespace art_gallery.Models
{
    public class Art
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Artist { get; set; }
        public DateTime Date_Of_Work { get; set; }
        public string Style { get; set; }
        public double[] Dimensions { get; set; }
        public decimal EstimatedValue { get; set; }
    }
}
