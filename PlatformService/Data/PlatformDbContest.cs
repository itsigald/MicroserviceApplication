using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformDbContest : DbContext
    {
        public PlatformDbContest(DbContextOptions<PlatformDbContest> opt) : base(opt)
        {
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}
