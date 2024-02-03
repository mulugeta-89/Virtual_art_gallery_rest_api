using art_gallery.Models;

namespace art_gallery.Interfaces
{
    public interface ISoloExhibitionService
    {
        Task<List<SoloExhibition>> GetAllAsync();
        Task<SoloExhibition> GetByIdAsync(string id);
        Task<List<SoloExhibition>> GetSpecificAsync(string ownerId);
        Task CreateAsync(SoloExhibition soloExhibition);
        Task UpdateAsync(string id, SoloExhibition soloExhibition);
        Task DeleteAsync(string id);
    }
}
