﻿using art_gallery.Models;

namespace art_gallery.Interfaces
{
    public interface IArtsService
    {
        Task<List<Art>> GetAsync();
        Task<List<Art>> GetPublicAsync();
        Task<Art?> GetAsync(string id);
        Task<List<Art>> GetSpecificAsync(string ownerId);
        Task CreateAsync(Art newArt);
        Task UpdateAsync(string id, Art updatedArt);
        Task RemoveAsync(string id);
    }
}
