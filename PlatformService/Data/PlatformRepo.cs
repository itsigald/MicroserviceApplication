using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using System.Reflection.Metadata.Ecma335;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly PlatformDbContest _contest;

        public PlatformRepo(PlatformDbContest contest)
        {
            _contest = contest;
        }

        public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
        {
            return await _contest.Platforms.ToListAsync();
        }

        public async Task<Platform?> GetPlatfomByIdAsync(int id)
        {
            return await _contest.Platforms.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void AddPlatform(Platform? platform)
        {
            if (platform == null)
                throw new ArgumentException($"Create Platform: {nameof(platform)} is null");

            _contest.Platforms.Add(platform);
        }

        public void UpdatePlatform(Platform? platform)
        {
            if (platform == null)
                throw new ArgumentException($"Update Platform: {nameof(platform)} is null");

            _contest.Platforms.Update(platform);
        }

        public void DeletePlatform(Platform? platform)
        {
            if (platform == null)
                throw new ArgumentException($"Delete Platform: {nameof(platform)} is null");

            _contest.Remove(platform);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _contest.SaveChangesAsync() > 0;
        }
    }
}
