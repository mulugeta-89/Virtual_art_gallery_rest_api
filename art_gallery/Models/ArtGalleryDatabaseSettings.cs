﻿namespace art_gallery.Models
{
    public class ArtGalleryDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ArtsCollectionName { get; set; } = null!;
        public string ExhibitionsCollectionName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string RolesCollectionName { get; set; } = null!;
    }
}
