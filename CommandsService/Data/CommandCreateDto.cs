using System.ComponentModel.DataAnnotations;

namespace CommandsService.Data
{
    public class CommandCreateDto
    {
        public string? HowTo { get; set; }

        public string? Commandline { get; set; }
    }
}
