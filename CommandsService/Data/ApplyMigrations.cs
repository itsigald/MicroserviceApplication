using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public static class ApplyMigrations
    {
        public static void RegisterMigrations(IApplicationBuilder app, IWebHostEnvironment environment, Serilog.ILogger logger)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                seedData(serviceScope.ServiceProvider.GetService<CommandDbContext>(), logger, environment.IsProduction());
            }
        }

        private static void seedData(CommandDbContext? context, Serilog.ILogger logger, bool isProduction)
        {
            if (context == null)
            {
                logger.Information($"--> The context {nameof(context)} is null");
                throw new ArgumentNullException($"--> The context {nameof(context)} is null");
            }

            if (isProduction)
            {
                logger.Information($"--> Apply migration for {nameof(context)}");

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.Information($"--> Error on migration: {ex.Message}");
                    throw;
                }
            }
            else
            {
                logger.Information("--> We already have data");
            }

        }
    }
}
