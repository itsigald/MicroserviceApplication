using CommandsService.Dtos;

namespace CommandsService.Services
{
    public class SettingService : ISettingService
    {
        private Setting _setting;

        public SettingService(Setting? setting)
        {
            if (setting == null)
                throw new ArgumentNullException($"Parameter {nameof(setting)} is null");

            _setting = setting;
        }

        public Setting? GetSetting
        {
            get { return _setting; }
        }
    }
}
