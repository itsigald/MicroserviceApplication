using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using PlatformService.Services;

namespace PlatformService.Data
{
    public class PlatformDbContest : DbContext
    {
        private readonly ISettingService _settings;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<PlatformDbContest> _logger;

        public PlatformDbContest(DbContextOptions<PlatformDbContest> opt, ISettingService setting, IWebHostEnvironment env, ILogger<PlatformDbContest> logger) : base(opt)
        {
            _settings = setting;
            _env = env;
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_env.IsDevelopment())
            {
                _logger.LogInformation("Using DB InMemory");
                optionsBuilder.UseInMemoryDatabase("PlatformInMemory");
            }
            else
            {
                _logger.LogInformation("Using DB SqlServer");
                optionsBuilder.UseSqlServer(_settings.GetSetting?.ConnectionString);
            }
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}
