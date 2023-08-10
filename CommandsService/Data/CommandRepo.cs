using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly CommandDbContext _context;

        public CommandRepo(CommandDbContext contest)
        {
            _context = contest;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;
            command.Platform = null;

            _context.Commands.Add(command);
        }

        public void UpdateCommand(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _context.Commands.Update(command);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await _context.Platforms.ToListAsync();
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform == null) 
                throw new ArgumentNullException(nameof(platform));

            _context.Platforms.Add(platform);
        }

        public async Task<Command?> GetCommand(int platformId, int commandId)
        {
            return await _context.Commands
                .FirstOrDefaultAsync(
                    c => c.PlatformId == platformId &&
                    c.Id == commandId
                );
        }

        public async Task<IEnumerable<Command>> GetCommandForPlatform(int platformId)
        {
            return await _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform == null ? null : c.Platform.Name)
                .ToListAsync();
        }

        public async Task<bool> PlatformExists(int platformExternalId)
        {
            return await _context.Platforms.AnyAsync(p => p.ExternalId == platformExternalId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExternalPlatformExists(int externalPlatformId)
        {
            return await _context.Platforms.AnyAsync(p => p.ExternalId == externalPlatformId);
        }
    }
}
