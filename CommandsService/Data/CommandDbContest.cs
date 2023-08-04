using CommandsService.Dtos;
using CommandsService.Modals;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class CommandDbContest : DbContext
    {
        private readonly ISettingService _settings;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CommandDbContest> _logger;

        public CommandDbContest(DbContextOptions<CommandDbContest> opt, ISettingService setting, IWebHostEnvironment env, ILogger<CommandDbContest> logger) : base(opt)
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
                optionsBuilder.UseInMemoryDatabase("CommandInMemory");
            }
            else
            {
                _logger.LogInformation("Using DB SqlServer");
                optionsBuilder.UseSqlServer(_settings.GetSetting?.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Platform>()
                .HasMany(p => p.Commands)
                .WithOne(p => p.Platform)
                .HasForeignKey(p => p.PlatformId);

            modelBuilder
                .Entity<Command>()
                .HasOne(p => p.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.PlatformId);
        }

        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Command> Commands { get; set; }
    }
}
