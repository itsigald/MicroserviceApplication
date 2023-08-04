using CommandsService.Modals;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        // metodi per Platforms
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Platform>> GetAllPlatforms();

        void CreatePlatform(Platform platform);

        Task<bool> PlatformExists(int platformId);

        // metodi per Commands
        Task<IEnumerable<Command>> GetCommandForPlatform(int platformId);

        Task<Command?> GetCommand(int platformId, int commandId);

        void CreateCommand(int platformId, Command command);

        void UpdateCommand(Command command);
    }
}
