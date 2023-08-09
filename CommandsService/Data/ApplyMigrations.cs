using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public static class ApplyMigrations
    {
        public static void RegisterMigrations(IApplicationBuilder app, IWebHostEnvironment environment, Serilog.ILogger logger)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                seedData(serviceScope.ServiceProvider.GetService<CommandDbContest>(), logger, environment.IsProduction());
            }
        }

        private static void seedData(CommandDbContest? context, Serilog.ILogger logger, bool isProduction)
        {
            if (context == null)
                throw new ArgumentNullException($"The context {nameof(context)} is null");

            if (isProduction)
            {
                logger.Information($"--------> Apply migration for...");

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.Information($"--------> Error on migration: {ex.Message}...");
                    throw;
                }
            }
            else
            {
                logger.Information("--------> We already have data");
            }

        }
    }
}
