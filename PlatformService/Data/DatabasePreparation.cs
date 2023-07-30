using Microsoft.Extensions.Configuration.UserSecrets;
using PlatformService.Models;
using Serilog;
using Serilog.Core;
using System.Diagnostics;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace PlatformService.Data
{
    public static class DatabasePreparation
    {
        public static void PrepPolulation(IApplicationBuilder app, Serilog.ILogger logger)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                seedData(serviceScope.ServiceProvider.GetService<PlatformDbContest>(), logger);
            }
        }

        private static void seedData(PlatformDbContest? contest, Serilog.ILogger logger)
        {
            if (contest == null)
                throw new ArgumentNullException($"The context {nameof(contest)} is null");

            if(!contest.Platforms.Any())
            {
                //Console.Write("--------> Seeding data...");
                logger.Information("--------> Seeding data...");

                contest.Platforms.AddRange(
                    new Platform { Name = "Dotnet", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "DockerHub", Publisher = "Docker", Cost = "Free" },
                    new Platform { Name = "SqlServer", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "RabbitMQ", Publisher = "Rabbit Inc", Cost = "Free" },
                    new Platform { Name = "Oracle Database", Publisher = "Oracle", Cost = "100000" }
                );

                int insertRows = contest.SaveChanges();

                logger.Information($"--------> Seeded { insertRows } data...");
            }
            else
            {
                //Console.Write("--------> We already data");
                logger.Information("--------> We already data");
            }

        }
    }
}
