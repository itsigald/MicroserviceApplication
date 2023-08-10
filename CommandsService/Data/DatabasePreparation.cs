using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public static class DatabasePreparation
    {
        public static void PreparationPopulation(IApplicationBuilder builder, Serilog.ILogger logger)
        {
            using(var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var context = serviceScope.ServiceProvider.GetService<CommandDbContext>();

                if (grpcClient != null)
                {
                    var platforms = grpcClient.ReturnAllPlatform();
                    SeedData(context, platforms, logger);
                }
                else
                {
                    logger.Error("The GRPC client is null");
                }

            }
        }

        private static void SeedData(CommandDbContext? context, IEnumerable<Platform>? platforms, Serilog.ILogger logger)
        {
            if(platforms != null && context != null)
            {
                logger.Information($"--> Start of Create platforms");

                foreach (var platform in platforms)
                {
                    if(!context.Platforms.Any(p => p.ExternalId == platform.ExternalId))
                    {
                        context.Platforms.Add(platform);
                        logger.Information($"--> Adding platform { platform.Name }");
                    }
                }

                context.SaveChanges();

                logger.Information($"--> End of Create platforms");
            }
        }

    }
}
