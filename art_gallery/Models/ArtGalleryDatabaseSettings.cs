namespace art_gallery.Models
{
    public class ArtGalleryDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ArtsCollectionName { get; set; } = null!;
    }
}
