using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        Task<IEnumerable<Platform>> GetAllPlatformsAsync();

        Task<Platform?> GetPlatfomByIdAsync(int id);

        void AddPlatform(Platform? platform);

        void UpdatePlatform(Platform? platform);

        void DeletePlatform(Platform? platform);
        
        Task<bool> SaveChangesAsync();
    }
}
