using PlatformService.Dtos;

namespace PlatformService.Services
{
    public interface ISettingService
    {
        Setting? GetSetting { get; }
    }
}
